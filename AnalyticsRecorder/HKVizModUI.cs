using Modding;
using Satchel.BetterMenus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalyticsRecorder {
    internal class HKVizModUI: Loggable {
        private static HKVizModUI instance;

        public static HKVizModUI Instance {
            get {
                if (instance == null) {
                    instance = new HKVizModUI();
                }
                return instance;
            }
        }


        private Menu MenuRef;

        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates) {
            MenuRef ??= new Menu(
                name: "HKViz",
                elements: new Element[]
                {
                        new MenuButton(
                            name: "Login to " + Constants.WEBSITE_URL,
                            description: "So analytics files can be uploaded and visualized",
                            submitAction: (_) => HKVizAuthManager.Instance.Login()
                        ),
                }
            );

            return MenuRef.GetMenuScreen(modListMenu);
        }
    }
}
