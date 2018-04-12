using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PCME.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Login")]
    public class LoginController : Controller
    {
        //[HttpGet]
        [Route("[action]")]
        public IActionResult GetSystemInfo()
        {
            Dictionary<string, object> cfg = new Dictionary<string, object>();
            Dictionary<string, object> company = new Dictionary<string, object>();
            Dictionary<string, object> systeminfo = new Dictionary<string, object>();
            company.Add("companyid", "1");
            company.Add("companyname", "卫盛科技");
            company.Add("companylongname", "淄博卫盛科技有限公司");
            company.Add("address", "人民西路45号");
            company.Add("linkmen", "唐林");
            company.Add("telnumber", "18653311771");
            company.Add("servicedepartment", "研发部");
            company.Add("servicemen", "唐唐");
            company.Add("servicetelnumber", "18653311771");
            company.Add("serviceqq", "3447063");
            company.Add("serviceemail", "3447063@qq.com");
            company.Add("servicehomepage", "http://www.zbwsrc.cn");

            systeminfo.Add("systemname", "继续教育管理系统");
            systeminfo.Add("systemversion", "ver 1.0.0.0");
            systeminfo.Add("iconurl", "");
            systeminfo.Add("iconcls", "");
            systeminfo.Add("systemaddition", "");
            systeminfo.Add("copyrightowner", "唐林");
            systeminfo.Add("copyrightinfo", "卫盛科技2018");
            systeminfo.Add("allowsavepassword", "");
            systeminfo.Add("savepassworddays", "");
            systeminfo.Add("needidentifingcode", "");
            systeminfo.Add("alwaysneedidentifingcode","");
            systeminfo.Add("forgetpassword", "忘记密码找管理员");

            cfg.Add("systeminfo", systeminfo);
            cfg.Add("company", company);
            return Ok(cfg);
        }
    }
}
