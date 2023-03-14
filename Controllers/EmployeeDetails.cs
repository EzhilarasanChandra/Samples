using BAL_SampleApplication;
using BAL_SampleApplication.Interface;
using DAL_SampleApplication.Interface;
using DAL_SampleApplication.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SampleApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeDetails : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeDetails(IEmployeeRepository employeeRepository, IEmployeeService employeeService)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }
        [HttpGet("GetEmployeeById")]
        public Emp_Table GetById(int Id)
        {
            var data = _employeeService.GetPersonByUserId(Id);
            return data;
        }
        [HttpGet("GetAllEmployees")]
        public List<Emp_Table> GetAll()
        {
            var data = _employeeService.GetEmployees();
            return data;
        }
        [HttpPost("AddEmployee")]
        public async Task<Emp_Table> AddPerson([FromBody] Emp_Table employee)
        {
            return await _employeeService.AddEmployee(employee);
        }


        //public ActionResult Index(EncryptModel obj)
        //{
        //    int req = Convert.ToInt32(Request.Form["type"]);
        //    if (req == 1)
        //    {
        //        //ViewBag.Result = Encryptword(obj.Text);
        //    }
        //    else
        //    {
        //        //ViewBag.Result = Decryptword(obj.Text);
        //    }
        //    return View(obj);
        //}
        [HttpGet("EnCrypt")]
        public string Encryptword(string FirstName)
        {
            var data = _employeeService.GetEncrypt(FirstName);
            string key = "1prt56";
            byte[] SrctArray;
            byte[] EnctArray = UTF8Encoding.UTF8.GetBytes(FirstName);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider obj1 = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objcrypt = new MD5CryptoServiceProvider();
            SrctArray = objcrypt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objcrypt.Clear();
            obj1.Key = SrctArray;
            obj1.Mode = CipherMode.ECB;
            obj1.Padding = PaddingMode.PKCS7;
            ICryptoTransform crptotrns = obj1.CreateEncryptor();
            byte[] resArray = crptotrns.TransformFinalBlock(EnctArray, 0, EnctArray.Length);
            obj1.Clear();
            return Convert.ToBase64String(resArray, 0, resArray.Length);
        }
        [HttpPost("Decrypt")]
        public string Decryptword(string DecryptText)
        {
            string key = "1prt56";
            byte[] SrctArray;
            byte[] DrctArray = Convert.FromBase64String(DecryptText);
            SrctArray = UTF8Encoding.UTF8.GetBytes(key);
            TripleDESCryptoServiceProvider obj1 = new TripleDESCryptoServiceProvider();
            MD5CryptoServiceProvider objDCrypt = new MD5CryptoServiceProvider();
            SrctArray = objDCrypt.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            objDCrypt.Clear();
            obj1.Key = SrctArray;
            obj1.Mode = CipherMode.ECB;
            obj1.Padding = PaddingMode.PKCS7;
            ICryptoTransform cryptoTransform = obj1.CreateDecryptor();
            byte[] resArray = cryptoTransform.TransformFinalBlock(DrctArray, 0, DrctArray.Length);
            objDCrypt.Clear();
            return UTF8Encoding.UTF8.GetString(resArray);

        }
    }
}
