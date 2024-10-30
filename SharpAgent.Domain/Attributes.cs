using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Domain.Attributes;

public class ServiceCommandAttribute : Attribute
{
    public string ServiceType { get; }
    public ServiceCommandAttribute(string serviceType) => ServiceType = serviceType;
}
