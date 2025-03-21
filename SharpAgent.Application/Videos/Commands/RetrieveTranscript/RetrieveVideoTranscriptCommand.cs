using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Application.Videos.Commands.RetrieveTranscript;

public class RetrieveVideoTranscriptCommand : IRequest<Guid?>
{
    public Guid VideoId { get; set; }
}
