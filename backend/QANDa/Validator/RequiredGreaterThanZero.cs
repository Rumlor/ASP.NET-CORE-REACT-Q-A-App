using System.ComponentModel.DataAnnotations;

namespace QANDa.Validator
{
    public class RequiredGreaterThanZero:ValidationAttribute
    {
        public RequiredGreaterThanZero() { }
        public override bool IsValid(object value)
        {
            int comparingInt;
            var result =  value !=null && int.TryParse(value.ToString(), out comparingInt) && comparingInt > 0;
            return result;
        }
    }
}
