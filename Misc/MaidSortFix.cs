using System;
using UnityEngine;
using UnityInjector;
using UnityInjector.Attributes;

namespace CM3D2.MaidSortFix.Plugin
{
    [PluginName("Maid Sort Fix"), PluginVersion("1.0")]
    public class MaidSortFix : PluginBase
    {
        public void Awake()
        {
            Console.WriteLine("########################################################################");
            Console.WriteLine("##########                  Maid Sort Fix loaded!             ##########");
            Console.WriteLine("########## Load the save file and press N to reset maid order ##########");
            Console.WriteLine("########################################################################");
        }

        public static int MaidCompareCreateTime(Maid x, Maid y)
        {
            int result;
            if (x.Param.status.create_time_num < y.Param.status.create_time_num)
                result = -1;
            else
                result = x.Param.status.create_time_num == y.Param.status.create_time_num ? 0 : 1;
            return result;
        }

        public void Update()
        {
            if (!Input.GetKeyDown(KeyCode.N))
                return;
            Console.WriteLine("Fixing maid order...");
            GameMain.Instance.CharacterMgr.GetStockMaidList().Sort(MaidCompareCreateTime);
            Console.WriteLine(
            "Done! Save your game and restart CM3D2. Remember to remove this plugin after you don't need it!");
        }
    }
}