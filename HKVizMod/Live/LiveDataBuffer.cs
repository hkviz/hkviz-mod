using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HKViz.Live {
    internal class LiveDataBuffer {
        private static LiveDataBuffer? _instance;
        public static LiveDataBuffer Instance {
            get {
                if (_instance != null) return _instance;
                _instance = new LiveDataBuffer();
                return _instance;
            }
        }

        private bool IsGettingContents = false;

        public bool IsLive = false;

        private StringBuilder _buffer2 = new StringBuilder();
        private StringBuilder _buffer = new StringBuilder();

        public void Append(string s) {
            _buffer.Append(s);
        }

        public string GetContentsAndClear() {
            var buffer = _buffer;
            _buffer = _buffer2;
            _buffer2 = buffer;
            var str = buffer.ToString();
            buffer.Clear();
            return str;
        }
    }
}
