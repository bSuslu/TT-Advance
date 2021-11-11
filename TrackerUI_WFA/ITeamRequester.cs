using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLibrary.Models;

namespace TrackerUI_WFA
{
    public interface ITeamRequester
    {
        void TeamComplete(TeamModel model);
    }
}
