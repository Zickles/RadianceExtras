

using System;
using System.Collections;
using System.Collections.Generic;
using Modding;
using Satchel;
using Satchel.BetterMenus;
using HutongGames.PlayMaker;
using Modding.Utils;
using UnityEngine;

namespace Mod1
{
    public class DummyComponent : MonoBehaviour { }
    public class GlobalSettings { public int xyn; public int cyn; public int myn; public int syn; public int swordamount; public int miniamount; }
    public class RadianceExtras : Mod, ICustomMenuMod, IGlobalSettings<GlobalSettings>
    {

        private GameObject[] _swords = new GameObject[4];
        private GameObject watchers = null;
        private GameObject shields = null;

        private GameObject hunter = null;

        new public string GetName() => "RadianceExtras";
        public override string GetVersion() => "1.0.0";

        public override List<(string, string)> GetPreloadNames()
        {
            return new List<(string, string)>
    {
        ("RestingGrounds_02_boss", "Warrior/Ghost Warrior Xero/Sword 1"),
        ("RestingGrounds_02_boss", "Warrior/Ghost Warrior Xero/Sword 2"),
        ("RestingGrounds_02_boss", "Warrior/Ghost Warrior Xero/Sword 3"),
        ("RestingGrounds_02_boss", "Warrior/Ghost Warrior Xero/Sword 4"),
        ("Mines_07","Crystal Flyer"),
        ("GG_Watcher_Knights","Battle Control/Black Knight 1"),
        ("sharedassets314", "Markoth Shield"),
    };
        }

        enum Xswords
        {
            Yes,
            No,
        }

        enum Chunters
        {
            Yes,
            No,
        }

        enum Mknights
        {
            Yes,
            No,
        }
        enum Markshields
        {
            Yes,
            No,
        }

        enum Aswords
        {
            Easy,
            Hard,
        }

        enum MiniWatchers
        {
            Easy,
            Hard,
            Supreme,
        }


        Xswords xyn = Xswords.Yes;
        Chunters cyn = Chunters.Yes;
        Mknights myn = Mknights.Yes;
        Markshields syn = Markshields.Yes;

        Aswords swordamount = Aswords.Easy;
        MiniWatchers miniamount = MiniWatchers.Easy;



        int Swordindex = 2;
        int Watcherindex = 2;

        public void OnLoadGlobal(GlobalSettings s)
        {
            xyn = (Xswords)s.xyn;
            cyn = (Chunters)s.cyn;
            myn = (Mknights)s.myn;
            syn = (Markshields)s.syn;

            swordamount = (Aswords)s.swordamount;
            Swordindex = (s.swordamount + 1) * 2;

            miniamount = (MiniWatchers)s.miniamount;
            Watcherindex = (s.miniamount + 1) * 2;
            Log(Watcherindex);
        }


        public GlobalSettings OnSaveGlobal() => new GlobalSettings() { xyn = (int)xyn, cyn = (int)cyn, myn = (int)myn, syn = (int)syn, swordamount = (int)swordamount, miniamount = (int)miniamount };

        // a variable that holds our Satchel.BetterMenu.Menu for us to use in the code.
        private Menu HomeMenu;
        private Menu ToggleMenu;
        private Menu SettingsMenu;

        public bool ToggleButtonInsideMenu => true;

        //function required to be created when inheriting from ICustomMenuMod.
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates)
        {


            //Create a new MenuRef if it's not null
            HomeMenu ??= new Menu(
                            "Radiance Extra", //the title of the menu screen, it will appear on the top center of the screen 
                            new Element[]
                            {

                                Blueprints.NavigateToMenu("Toggle Menu",
                                "Go to Toggle Page",
                                () => ToggleMenu.GetMenuScreen(HomeMenu.menuScreen)), //this is a Func<MenuScreen> you have to return the "Next Page" MenuScreen here

                                 Blueprints.NavigateToMenu("Settings Menu",
                                "Go to Settings Page",
                                () => SettingsMenu.GetMenuScreen(HomeMenu.menuScreen)), //this is a Func<MenuScreen> you have to return the "Next Page" MenuScreen here

                            }




                );

            ToggleMenu ??= new Menu(
                "My Extra Menu",
                        new Element[]
                        {
                                       new HorizontalOption("Xero Swords",
                                            "If you want them in the fight or not",
                                             Enum.GetNames(typeof(Xswords)), //get an string array of all values in Modes enum
                                        (index) =>
                                        {
                                            xyn = (Xswords)index;
                                        },
                                        () => (int)xyn),

                                       new HorizontalOption("Craystal Hunters",
                                            "If you want them in the fight or not",
                                             Enum.GetNames(typeof(Chunters)), //get an string array of all values in Modes enum
                                        (index) =>
                                        {
                                            cyn = (Chunters)index;
                                        },
                                        () => (int)cyn),

                                       new HorizontalOption("Mini Watcher Knights",
                                            "If you want them in the fight or not",
                                             Enum.GetNames(typeof(Mknights)), //get an string array of all values in Modes enum
                                        (index) =>
                                        {
                                            myn = (Mknights)index;
                                        },
                                        () => (int)myn),

                                        new HorizontalOption("Markoth Shield",
                                            "If you want them in the fight or not",
                                             Enum.GetNames(typeof(Markshields)), //get an string array of all values in Modes enum
                                        (index) =>
                                        {
                                            syn = (Markshields)index;
                                        },
                                        () => (int)syn),

                        });
            SettingsMenu ??= new Menu(
                 "My Extra Menu",
                        new Element[]
                        {
                            new HorizontalOption("Sword Amount",
                                            "Amount of Xero Swords You Want in the Fight",
                                             Enum.GetNames(typeof(Aswords)), //get an string array of all values in Modes enum
                                        (index) =>
                                        {
                                           swordamount = (Aswords)index;

                                            switch (swordamount)
                                            {
                                                case Aswords.Easy:
                                                    {
                                                        int Swordindex = 2;
                                                        break;
                                                    }
                                                case Aswords.Hard:
                                                    {
                                                        int Swordindex = 4;
                                                        break;
                                                    }
                                            }





                                        },
                                        () => (int)swordamount),

                                        new HorizontalOption("Mini Watchers Amount",
                                            "Goes up by 2 with each option",
                                             Enum.GetNames(typeof(MiniWatchers)), //get an string array of all values in Modes enum
                                        (index) =>
                                        {
                                           miniamount = (MiniWatchers)index;

                                            switch (miniamount)
                                            {
                                                case MiniWatchers.Easy:
                                                    {
                                                        int Watcherindex = 2;
                                                        break;
                                                    }
                                                case MiniWatchers.Hard:
                                                    {
                                                        int Watcherindex = 4;
                                                        break;
                                                    }
                                                case MiniWatchers.Supreme:
                                                    {
                                                        int Watcherindex = 6;
                                                        break;
                                                    }
                                            }


                                        },
                                        () => (int)miniamount),



                        });

            //uses the GetMenuScreen function to return a menuscreen that MAPI can use. 
            //The "modlistmenu" that is passed into the parameter can be any menuScreen that you want to return to when "Back" button or "esc" key is pressed 
            HomeMenu.GetMenuScreen(modListMenu);
            ToggleMenu.GetMenuScreen(HomeMenu.menuScreen);
            SettingsMenu.GetMenuScreen(HomeMenu.menuScreen);
            return HomeMenu.GetMenuScreen(modListMenu);
        }




        private void PlayMakerFSM_OnEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            if (self.gameObject.name == "Absolute Radiance" && self.FsmName == "Attack Choices")
            {
                self.InsertAction("Init", new Satchel.Futils.CustomFsmAction()
                {
                    method = () =>
                    {




                        switch (cyn)
                        {
                            case Chunters.Yes:
                                GameObject newHunter = GameObject.Instantiate(hunter);
                                newHunter.GetComponent<HealthManager>().hp = int.MaxValue;
                                newHunter.SetActive(true);
                                newHunter.transform.position = new Vector3(67f, 28f, newHunter.transform.position.z);
                                break;
                            case Chunters.No:
                                break;


                        }

                        switch (xyn)
                        {
                            case Xswords.Yes:
                                for (int i = 0; i < Swordindex; i++)
                                {
                                    GameObject sword = _swords[i];
                                    GameObject newSword = GameObject.Instantiate(sword, self.transform);
                                    newSword.transform.localPosition = new Vector3((float)i - 1.5f, 0f, newSword.transform.localPosition.z);
                                    GameObject swordHome = new GameObject($"S{i + 1} Home");
                                    swordHome.transform.parent = self.transform;
                                    swordHome.transform.localPosition = newSword.transform.localPosition;

                                    PlayMakerFSM fsm = newSword.LocateMyFSM("xero_nail");
                                    newSword.SetActive(true);
                                    newSword.AddComponent<DummyComponent>().StartCoroutine(SwordRoutine());

                                    var dmgHero = newSword.GetComponent<DamageHero>();
                                    dmgHero.damageDealt = 3;

                                    IEnumerator SwordRoutine()
                                    {
                                        while (true)
                                        {
                                            yield return new WaitForSeconds(UnityEngine.Random.Range(2, 4));
                                            fsm.SendEvent("ATTACK");
                                        }
                                    }
                                }
                                break;
                            case Xswords.No:
                                break;


                        }

                        switch (myn)
                        {
                            case Mknights.Yes:
                                for (int i = 0; i < Watcherindex; i++)
                                {
                                    GameObject newWatchers = GameObject.Instantiate(watchers);
                                    newWatchers.SetActive(true);
                                    newWatchers.AddComponent<DummyComponent>().StartCoroutine(WatcherFall());
                                    newWatchers.GetOrAddComponent<Rigidbody2D>().gravityScale = 2f;
                                    newWatchers.GetComponent<Rigidbody2D>().isKinematic = false;
                                    newWatchers.transform.position = new Vector3(60f, 23f, newWatchers.transform.position.z);
                                    GameObject.Destroy(newWatchers.transform.GetChild(0).gameObject);
                                    GameObject.Destroy(newWatchers.transform.GetChild(1).gameObject);
                                    Fsm fsm = newWatchers.LocateMyFSM("Black Knight").Fsm;
                                    FsmTransition transition = fsm.GetState("Init Facing").Transitions[0];
                                    transition.ToState = "Idle";
                                    transition.ToFsmState = fsm.GetState("Idle");

                                    IEnumerator WatcherFall()
                                    {
                                        while (true)
                                        {
                                            var watcherpos = newWatchers.transform.position;
                                            if (watcherpos.y < 18)
                                            {
                                                newWatchers.transform.position = new Vector3(60f, 23f, newWatchers.transform.position.z);

                                            }
                                            if (newWatchers.transform.localScale.y == 1)
                                            {
                                                newWatchers.transform.localScale *= 0.2f;
                                            }
                                            yield return null;
                                        }
                                    }

                                }
                                break;
                            case Mknights.No:
                                break;
                        }

                        switch(syn)
                        {
                            case Markshields.Yes:
                        GameObject newShields = GameObject.Instantiate(shields, self.transform);
                        PlayMakerFSM sfsm = newShields.LocateMyFSM("Sheild Attack");
                        newShields.SetActive(true);
                        newShields.transform.GetChild(1).gameObject.SetActive(true);
                        sfsm.GetState("Idle");
                        sfsm.SetState("Idle");

                                break;
                            case Markshields.No:
                                break;
                    }





                    }
                }, 0);
            }
            orig(self);
        }














        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {

            /*ModHooks.HeroUpdateHook += OnHeroUpdate;*/
            ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;
            On.PlayMakerFSM.OnEnable += PlayMakerFSM_OnEnable;

            hunter = preloadedObjects["Mines_07"]["Crystal Flyer"];
            UnityEngine.Object.DontDestroyOnLoad(hunter);

            watchers = preloadedObjects["GG_Watcher_Knights"]["Battle Control/Black Knight 1"];
            UnityEngine.Object.DontDestroyOnLoad(watchers);

            for (int index = 0; index < 4; index++)
            {
                _swords[index] = preloadedObjects["RestingGrounds_02_boss"]["Warrior/Ghost Warrior Xero/Sword " + (index + 1)];
                UnityEngine.Object.DontDestroyOnLoad(_swords[index]);
            }

            shields = preloadedObjects["sharedassets314"]["Markoth Shield"];
            UnityEngine.Object.DontDestroyOnLoad(shields);

        }

        private string ModHooks_LanguageGetHook(string key, string sheetTitle, string orig)
        {
            switch (key)
            {
                case "ABSOLUTE_RADIANCE_SUPER":
                    return "MOST ANNOYING UNSACRIFICIAL LIVING DIRTBAG BIG HEAD SMALL BRAIN BOZO";
                case "GG_S_RADIANCE":
                    return "God of Annoyingness.";
                case "GODSEEKER_RADIANCE_STATUE":
                    return "Ok.";

                /*You Beat it But Keep looking For More Updates to the mod, also dm me ideas Zickles#9116*/

                default:
                    return orig;
            }
        }


    }
}



   
