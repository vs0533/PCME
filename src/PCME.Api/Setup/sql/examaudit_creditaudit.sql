/*导入examaudit*/
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
examsubjects.Code,
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
