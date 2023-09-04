using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using EdgeDB.DataTypes;

namespace Reserve.Core.Features.Appointment;

public class Availability
{
    public Days Day { get; set; }
    public LocalTime StartTime { get; set; }
    public LocalTime EndTime { get; set; }
}

public enum Days
{
    Monday,
    Tuesday,
    Wednesday,
    Thursday,
    Friday,
    Saturday,
    Sunday
}
