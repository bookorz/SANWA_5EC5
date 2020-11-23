using System;
using System.Collections.Generic;
using System.Text;

namespace SanwaSecsDll
{
    class SanwaE05
    {
    }

    public static class E30_MF  //Material format code 1 byte by Format 10. 
    {
        public const byte IN_WAFERS = 0x01;          //1 = Quantities in wafers
        public const byte IN_CASSETTE = 0x02;        //2 = Quantities in cassette
        public const byte IN_DIE_OR_CHIPS = 0x03;    //3 = Quantities in die or chips
        public const byte IN_BOATS = 0x04;           //4 = Quantities in boats 
        public const byte IN_INGOTS = 0x05;          //5 = Quantities in ingots 
        public const byte IN_LEADFRAMES = 0x06;      //6 = Quantities in leadframes 
        public const byte IN_LOTS = 0x07;            //7 = Quantities in lots in magazines        
        public const byte IN_MAGAZINES = 0x08;        //8 = Quantities in magazines 
        public const byte IN_PACKAGES = 0x09;        //9 = Quantities in packages 
        public const byte IN_PLATES = 0x0a;          //10 = Quantities in plates 
        public const byte IN_TUBES = 0x0b;           //11 = Quantities in tubes 
        public const byte IN_WATERFRAMES = 0x0c;     //12 = Quantities in waterframes
        public const byte IN_CARRIERS = 0x0d;         //13 = Quantities in carriers 
        public const byte IN_SUBSTRATES = 0x0e;      //14 = Quantities in substrates
    };
}
