if not exists(select 1 from sys.servers where name='PCME')
begin
EXEC sp_addlinkedserver   'PCME ', ' ', 'SQLOLEDB ', '10.128.200.166' 
exec sp_addlinkedsrvlogin  'PCME ', 'false ',null, 'sa', 'abc28122661' 
END
--1390834
GO
--按照科目和人员汇总作业成绩并插入临时表#temp
SELECT homework.studentid,examsubject.id AS examsubjectid,SUM(homework.score) AS score,COUNT(homework.score) AS ctr INTO #temp FROM Homeworkresult homework
LEFT JOIN (select * 
FROM openquery(PCME,  'SELECT *  FROM PCME.DBO.ExamSubjects')) examsubject ON homework.categorycode = examsubject.Code
GROUP BY homework.studentid,examsubject.id

--取得未插入学分的准考证和汇总的作业 和违纪情况 并插入临时表#temp2
SELECT * INTO #temp2 from(
	  SELECT ExamResult.id,ExamResult.createtime,ExamResult.examsubjectid,ExamResult.score,ExamResult.studentid,ExamResult.ticketnum,
	  #temp.score AS homeworkscore,#temp.ctr AS homeworkctr,invigilate.Id AS invigilate,
		--(SELECT score FROM #temp WHERE studentid = PCME_TEST.DBO.ExamResult.studentid AND examsubjectid = PCME_TEST.DBO.ExamResult.Examsubjectid) AS homeworkscore,
		--(SELECT ctr FROM #temp WHERE studentid = PCME_TEST.DBO.ExamResult.studentid AND examsubjectid = PCME_TEST.DBO.ExamResult.Examsubjectid) AS homeworkctr,
		istoexamaudit
      FROM PCME_TEST.DBO.ExamResult 
	  LEFT JOIN invigilateforstudent invigilate ON invigilate.ticketNum = ExamResult.ticketNum
	  LEFT JOIN  #temp ON ExamResult.studentid = #temp.studentid AND #temp.examsubjectid = ExamResult.ExamSubjectId
    ) as tb
    WHERE tb.istoexamaudit = 0 AND tb.homeworkctr >=5
    ORDER BY id;
	--SELECT * FROM #temp2 ORDER BY id	
	--SELECT * FROM examresult
/*成绩导入学分*/
DECLARE tempCursor CURSOR
FOR
    (SELECT * FROM #temp2) ORDER BY id								--创建游标tempCursor，并定义游标所指向的集合   
OPEN tempCursor;								--打开游标 
DECLARE @id INT,@createtime datetime,@examsubjectid INT,@score float,@ticketnum nvarchar(20),@studentid float,@homeworkscore INT,@homeworkctr FLOAT,@istoexam BIT,@wj NVARCHAR(10); 
FETCH NEXT FROM tempCursor INTO @id,@createtime,@examsubjectid,@score,@studentid,@ticketnum,@homeworkscore,@homeworkctr,@wj,@istoexam		--游标读取下一个数据  
WHILE @@fetch_status = 0                        --游标读取下一个数据的状态，0表示读取成功  
    BEGIN  
        DECLARE @sum FLOAT
        SET @sum = @score+@homeworkscore
		--PRINT (convert(varchar(50),@id)+':'+convert(varchar(50),@createtime)+':'+convert(varchar(50),@examsubjectid)
		--+':'+convert(varchar(50),@studentid)+':'+convert(varchar(50),@ticketnum)
		--+':'+convert(varchar(50),@homeworkscore)+':'+convert(varchar(50),@score)
		--+':'+convert(varchar(50),@homeworkctr)+':'+convert(varchar(50),@sum)+':'+convert(varchar(50),@wj)
		--)	
		IF (@wj IS NULL) AND (@sum >=60)
        BEGIN
		  --begin transaction
			INSERT openquery(PCME,  'SELECT [AdmissionTicketNum],[CreateTime],[Credit],[StudentId],[SubjectId],[SumResult] FROM PCME.DBO.CreditExams_TEST')
			VALUES(@ticketnum,GETDATE(),20,@studentid,@examsubjectid,@sum)

			UPDATE ExamResult SET istoexamaudit = 1 WHERE TicketNum = @ticketnum
			--update a 
			--set a.AdmissionTicketNum = 'aaa'
			--from openrowset('SQLOLEDB' , '10.128.200.166' ; 'sa' ; 'abc28122661' ,PCME.dbo.CreditExams_TEST) as a
			--WHERE a.AdmissionTicketNum = @ticketnum
			--UPDATE OPENQUERY(PCME, 'select * from PCME.DBO.CreditExams_TEST') SET AdmissionTicketNum = 'asdf' WHERE AdmissionTicketNum=@ticketnum
			--判断事务的提交或者回滚
			--if(@@error<>0)
			--begin
			--	ROLLBACK transaction
			--	--RETURN -1 --设置操作结果错误标识
			--end
			--else
			--begin
			--	COMMIT transaction
			--	--RETURN 1 --操作成功的标识
			--end
		END
		
		FETCH NEXT FROM tempCursor INTO @id,@createtime,@examsubjectid,@score,@studentid,@ticketnum,@homeworkscore,@homeworkctr,@wj,@istoexam;    --继续用游标读取下一个数据  
    END  
CLOSE tempCursor;								--关闭游标
DEALLOCATE tempCursor;



--select * 
--FROM openquery(PCME,  'SELECT *  FROM PCME.DBO.CreditExams') 
go
DROP TABLE #temp
GO
DROP TABLE #temp2
go
if exists(select 1 from sys.servers where name='PCME')
begin
Exec sp_droplinkedsrvlogin PCME,NULL --删除链接服务器的登陆帐户
Exec sp_dropserver PCME --删除链接服务器
END
go
--查看已注册的链接服务器
--exec sp_linkedservers