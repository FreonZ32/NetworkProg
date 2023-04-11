using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace EFnetVisitations.Entities
{
    [Owned]
    internal class Passport
    {
        public string series { get; private set; }
        public string number { get; private set; }

        protected Passport() { }
        public Passport(string ser, string num)
        {
            if (ser == null) throw new ArgumentNullException(nameof(ser));
            else if (IsPassportSer(ser)) { series = ser; }
            else 
            { 
                series = "0000";
                throw new InvalidOperationException("Некорректная серия!");
            }
            if (num == null) throw new ArgumentNullException(nameof(num));
            else if (IsPassportNum(num)) { number = num; }
            else
            {
                number = "000000";
                throw new InvalidOperationException("Некорректный номер!");
            }
        }

        public string Series
        {
            get 
            {
                if (series == null) return "";
                return series;
            }
        }
        public string Number
        {
            get 
            {
                if (number == null) return "";
                return number; 
            }
        }
        public override string ToString()
        {
            return series + " " + number;
        }

        public static bool IsPassportSer(string ser)
        {
            if (ser.Length == 4 && int.TryParse(ser, out int sernum))
                return true;
            else return false;
        }
        public static bool IsPassportNum(string num)
        {
            if (num.Length == 6 && int.TryParse(num, out int numnum))
                return true;
            else return false;
        }
        protected bool Equals(Passport other)
        {
            if (series == other.series && number == other.number) return true;
            else return false;
        }
        public override bool Equals(object? obj)
        {
            if(ReferenceEquals(null, obj)) return false;
            if(ReferenceEquals(this, obj)) return true;
            if(obj.GetType() != typeof(Passport)) return false;
            return Equals((Passport)obj);
        }
        public static bool operator ==(Passport? left, Passport? right)
        {
            return Equals(left,right);
        }
        public static bool operator !=(Passport? left, Passport? right)
        { 
            return !Equals(left, right); 
        }
    }
}
