using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AccountSync.Models
{
    public class LoginViewModel:IValidatableObject
    {

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var DB_GEN_Repo= DB_GEN.RepositoryHelper.GetGenProxyAccountRepository();
            var user = DB_GEN_Repo.GetUser(this.UserID);
            if (user==null)
            {
                yield return new ValidationResult("帳號錯誤",new string[]{"帳號"});
            }
            else
            {
                var password = DB_GEN_Repo.GetMD5(this.Password.Trim()).ToUpper();

                if (user.chXData.ToUpper()!=DB_GEN_Repo.GetMD5(this.Password).ToUpper())
                {
                    yield return new ValidationResult("密碼錯誤", new string[] { "密碼" });
                }
            }
            if ((user.chUserName.Trim() != "曾義格") && (user.chUserName.Trim() != "蔡俊榮") && (user.chUserName.Trim() != "林文德") && (user.chUserName.Trim() != "潘靜瑩"))
            {
                yield return new ValidationResult("無權限", new string[] { "權限" });

            }
        }

        [DisplayFormat(ConvertEmptyStringToNull=false)]
        public string ReturnURL { get; set; }

        [Display(Name="帳號")]
        [Required]
        public string UserID { get; set; }

        [Display(Name="密碼")]
        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }


    }
}