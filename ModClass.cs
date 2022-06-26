using System;
using System.Collections;
using System.Collections.Generic;
using Modding;
using Satchel;
using Satchel.BetterMenus;
using UnityEngine;

namespace Mod1
{
    public class DummyComponent : MonoBehaviour { }
    public class GlobalSettings { public int swordamount; public int cyn; }
    public class RadianceExtras : Mod, ICustomMenuMod, IGlobalSettings<GlobalSettings>
    {

        private GameObject[] _swords = new GameObject[4];

        private GameObject hunter = null;

        new public string GetName() => "My First Mod";
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
    };
        }

        enum Aswords
        {
            Easy,
            Hard,
        }

        enum Chunters
        {
            Yes,
            No,
        }

        Aswords swordamount = Aswords.Easy;
        Chunters cyn = Chunters.Yes;

        public void OnLoadGlobal(GlobalSettings s)
        {
            swordamount = (Aswords)s.swordamount;
            Swordindex = (s.swordamount + 1) * 2;

            cyn = (Chunters)s.cyn;

        }


        public GlobalSettings OnSaveGlobal() => new GlobalSettings() { swordamount = (int)swordamount, cyn = (int)cyn };

        int Swordindex = 2;

        // a variable that holds our Satchel.BetterMenu.Menu for us to use in the code.
        private Menu MenuRef;

        public bool ToggleButtonInsideMenu => true;

        //function required to be created when inheriting from ICustomMenuMod.
        public MenuScreen GetMenuScreen(MenuScreen modListMenu, ModToggleDelegates? modtoggledelegates)
        {



            //Create a new MenuRef if it's not null
            MenuRef ??= new Menu(
                            "Radiance Extra", //the title of the menu screen, it will appear on the top center of the screen 
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


                                        new HorizontalOption("Craystal Hunters",
                                            "If you want them in the fight or not",
                                             Enum.GetNames(typeof(Chunters)), //get an string array of all values in Modes enum
                                        (index) =>
                                        {
                                            cyn = (Chunters)index;
                                        },
                                        () => (int)cyn)



                            }




                );

            //uses the GetMenuScreen function to return a menuscreen that MAPI can use. 
            //The "modlistmenu" that is passed into the parameter can be any menuScreen that you want to return to when "Back" button or "esc" key is pressed 
            return MenuRef.GetMenuScreen(modListMenu);
        }




        private void PlayMakerFSM_OnEnable(On.PlayMakerFSM.orig_OnEnable orig, PlayMakerFSM self)
        {
            if (self.gameObject.name == "Absolute Radiance" && self.FsmName == "Attack Choices")
            {
                self.InsertAction("Init", new Satchel.Futils.CustomFsmAction()
                {
                    method = () =>
                    {
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
                    }
                }, 0);
            }
            orig(self);
        }

        

        
       


        
   





        public override void Initialize(Dictionary<string, Dictionary<string, GameObject>> preloadedObjects)
        {

            Log(Swordindex);

            ModHooks.HeroUpdateHook += OnHeroUpdate;
            ModHooks.LanguageGetHook += ModHooks_LanguageGetHook;
            On.PlayMakerFSM.OnEnable += PlayMakerFSM_OnEnable;

            hunter = preloadedObjects["Mines_07"]["Crystal Flyer"];
            UnityEngine.Object.DontDestroyOnLoad(hunter);

            for (int index = 0; index < 4; index++)
            {
                _swords[index] = preloadedObjects["RestingGrounds_02_boss"]["Warrior/Ghost Warrior Xero/Sword " + (index + 1)];
                UnityEngine.Object.DontDestroyOnLoad(_swords[index]);
            }

           

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
                    return "Ok. You Beat it But Keep looking For More Updates to the mod, also dm me ideas Zickles#9116";



                default:
                    return orig;
            }
        }

        public void OnHeroUpdate()
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                Log("Key Pressed");
            }
        }

    }
}



    
