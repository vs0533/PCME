﻿/*导入examaudit*/
INSERT into PCME.dbo.CreditExams(admissionticketnum,sumresult,credit,createtime,studentid,subjectid)
select 
examaudit.examID,
examaudit.SumResult,
examaudit.CreditHour,
examaudit.ExamDate,
student.Id as studentid,
examsubjects.Id as examsubjectsid
from MOPDB.dbo.examAudit examaudit
left join MOPDB.dbo.Person person on examaudit.PersonID = person.personID
left join PCME.dbo.Students student on person.IDCard = student.IDCard
left join MOPDB.dbo.ExamSubject examsubject on LEFT(examaudit.ExamID,4) = examsubject.SubjectID
left join PCME.dbo.ExamSubjects examsubjects on examsubjects.Code = examsubject.SubjectID
where examaudit.CreditHour is not NULL

/*导入已经缴费但未合格的人员*/

insert into PCME.dbo.SignUp(createtime,examsubjectid,signupforunitid,studentid,ticketiscreate,trainingcenterid)
select * from 
(SELECT 
trainapply2.CreateDate,
examsubjects.Id,
null as signupforunitid,
students.Id as studentid,
0 as ticketiscreate,
trainapply2.TrainStationID
--trainapply2.PersonID,
--trainapply2.SubjectID,
--person.IDCard,
--examsubject.SubjectID as examsubjectid
 FROM MOPDB.DBO.TrainApply2 trainapply2
left join MOPDB.dbo.Person person on trainapply2.PersonID = person.personID
left join MOPDB.dbo.ExamSubject examsubject on trainapply2.SubjectID = examsubject.SubjectID
left join PCME.dbo.Students students on person.IDCard = students.IDCard
left join PCME.dbo.ExamSubjects examsubjects on examsubject.SubjectID = examsubjects.Code
where (trainapply2.Isenable =1 and trainapply2.Isenable2=1) and person.IDCard is not null and examsubject.SubjectID is not null) as tb

/*导入刊物信息*/
insert into PCME.dbo.Periodicals(level,logogram,name)
select AreaLevel,Code,PublicationName from MOPDB.DBO.Publication

/*导入论文著作*/
insert into PCME.dbo.Paper
select 
case when arealevelid is null then 1 else arealevelid end as arealevelid_,
auditAccount,auditstate,
case when awardpaperlevelid is null then 1 else awardpaperlevelid end as awardpaperlevelid_,creditHour,joincount,joinlevel,papername,periodicalID,publishdate,publishtypeId,tb.studentid  from
(
	select 
	(select top 1 id from PCME.DBO.AreaLevel where [name] = MOPDB.DBO.paperAudit.areaLevel) as arealevelid,
	auditAccount,
	case auditstate
	when 1 then 1
	when 2 then 3
	when 3 then 2
	end as auditstate,
	(select top 1 id from PCME.dbo.AwardPaperLevel where [name] = MOPDB.DBO.paperAudit.awardLevel) as awardpaperlevelid,
	creditHour,
	joincount,
	joinlevel,
	papername,
	(select top 1 Id from PCME.DBO.Periodicals where [name] =(select top 1 publicationname from MOPDB.DBO.Publication where publicationID = MOPDB.DBO.paperAudit.publicationID)) as periodicalID,
	publishdate,
	(select top 1 Id from PCME.DBO.PublishType where [Name] = MOPDB.DBO.paperAudit.paperType) as publishtypeId,
	Students.Id AS studentid
	 from MOPDB.DBO.paperAudit LEFT JOIN MOPDB.dbo.Person ON Person.personID = paperAudit.personID
	 LEFT JOIN PCME.DBO.Students ON Students.IDCard = Person.IDCard
) as tb
 /*导入科研成果*/
INSERT INTO PCME.DBO.ScientificPayoffs
SELECT 
CASE WHEN tb.arealevelid IS NULL THEN 1 ELSE tb.arealevelid END AS arealevelid,
tb.AuditAccount,tb.auditstateid,tb.awardsplevelid,tb.completeDate,
tb.creditHour,tb.joinLevel,tb.fruitName,tb.studentid
FROM 
(
	SELECT 
	(SELECT TOP 1 ID FROM PCME.DBO.AreaLevel WHERE [Name] = MOPDB.dbo.fruitAudit.areaLevel) AS arealevelid,
	AuditAccount,
	case auditstate
	when 1 then 1
	when 2 then 3
	when 3 then 2
	end as auditstateid,
	(SELECT TOP 1 ID FROM PCME.DBO.AwardSPLevel WHERE [Name] = MOPDB.dbo.fruitAudit.awardLevel) AS awardsplevelid,
	completeDate,
	creditHour,
	joinLevel,
	fruitName,
	PCME.dbo.Students.Id AS studentid
	FROM
	MOPDB.DBO.fruitAudit LEFT JOIN MOPDB.DBO.Person ON Person.personID = fruitAudit.personID
	LEFT JOIN PCME.DBO.Students ON Students.IDCard = Person.IDCard
) AS tb

/*导入学历/培训 类（学员申报）*/
INSERT INTO PCME.DBO.CreditTrains
SELECT 
	AuditAccount,
	case auditstate
	when 1 then 1
	when 2 then 3
	when 3 then 2
	end as auditstateid,
	creditHour,
	trainPeriod,
	frontUnit,
	PCME.dbo.Students.Id AS studentid,
	trainContent,trainDate,trainType
	FROM
	MOPDB.DBO.TrainAudit LEFT JOIN MOPDB.DBO.Person ON Person.personID = TrainAudit.personID
	LEFT JOIN PCME.DBO.Students ON Students.IDCard = Person.IDCard

	/*根据报名生成考试券*/
DECLARE tempCursor CURSOR
FOR
    (SELECT Id,CreateTime,'报名创建',
		replace(lower(Convert(varchar(100),NEWID())),'-',''),
		StudentId,TrainingCenterId
      FROM PCME.DBO.SignUp
    )
    ORDER BY id;								--创建游标tempCursor，并定义游标所指向的集合   
OPEN tempCursor;								--打开游标 
DECLARE @id INT,@createtime datetime,@remark nvarchar(10),@num nvarchar(40),@studentid int, @trainingcenterid int; 
FETCH NEXT FROM tempCursor INTO @id,@createtime,@remark,@num,@studentid,@trainingcenterid		--游标读取下一个数据  
WHILE @@fetch_status = 0                        --游标读取下一个数据的状态，0表示读取成功  
    BEGIN  
        PRINT (convert(varchar(50),@id)+':'+convert(varchar(50),@createtime))							--打印id
		INSERT INTO PCME.DBO.ExamRoomPlanTicket values(@createtime,NULL,0,@num,@remark,@studentid,@trainingcenterid)
		--SELECT * FROM HrmResource WHERE id = @id;
        FETCH NEXT FROM tempCursor INTO @id,@createtime,@remark,@num,@studentid,@trainingcenterid;    --继续用游标读取下一个数据  
    END  
CLOSE tempCursor;								--关闭游标
DEALLOCATE tempCursor;

/* 一些清空表数据
TRUNCATE TABLE dbo.ProfessionalInfos
TRUNCATE TABLE dbo.Paper
*/
