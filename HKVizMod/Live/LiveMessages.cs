using System;
using System.Collections.Generic;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKViz.Live {
    [Serializable]
    class HostAppendDataMessage {
		public string type = "host:append";
		public string data;

		public HostAppendDataMessage(string data) {
			this.data = data;
		}
    }

    [Serializable]
    class HostSwitchFileMessage {
		public string type = "host:switch";
		public long filePartNr;

		public HostSwitchFileMessage(long filePartNr) {
			this.filePartNr = filePartNr;
		}
	}

    [Serializable]
    class HostMarkFilePartUploadedMessage {
		public string type = "host:mark-uploaded";
		public long filePartNr;

		public HostMarkFilePartUploadedMessage(long filePartNr) {
			this.filePartNr = filePartNr;
		}
	}
}
