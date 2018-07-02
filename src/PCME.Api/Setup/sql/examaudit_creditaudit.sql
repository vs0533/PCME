insert into PCME.dbo.CreditExams(admissionticketnum,sumresult,credit,createtime,studentid,subjectid)
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
where examaudit.CreditHour is not null