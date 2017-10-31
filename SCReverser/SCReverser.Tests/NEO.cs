using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCReverser.Core.Enums;
using SCReverser.Core.Interfaces;
using SCReverser.Core.Types;
using SCReverser.NEO;
using SCReverser.NEO.Internals;
using System.IO;
using System.Linq;

namespace SCReverser.Tests
{
    [TestClass]
    public class NEO
    {
        [TestMethod]
        public void DebugTest()
        {
            NeoTemplate n = new NeoTemplate();

            NeoConfig ini = new NeoConfig(SmartContractSampleRaw)
            {
                TriggerType = ETriggerType.Application,
                BlockChainPath = null,
            };

            IReverser reverser = n.CreateReverser();
            ReverseResult rs = null;
            Assert.IsTrue(reverser.TryParse(ini, ref rs));

            using (IDebugger debugger = n.CreateDebugger(rs, ini))
            {
                bool bp1 = false, bp2 = false;
                // Check event
                debugger.OnBreakPoint += (d, instruction) =>
                {
                    // Remove BP
                    if (instruction.Location.Index == 1) bp1 = true;
                    else if (instruction.Location.Index == 2) bp2 = true;
                };

                // Add two demo BP
                debugger.Instructions[1].HaveBreakPoint = true;
                debugger.Instructions[2].HaveBreakPoint = true;

                // Run!

                debugger.Execute();

                // Check BP Executed

                Assert.IsTrue(debugger.CurrentInstruction == debugger.Instructions[1] && debugger.State.HasFlag(DebuggerState.BreakPoint));

                debugger.Execute();

                Assert.IsTrue(debugger.CurrentInstruction == debugger.Instructions[2] && debugger.State.HasFlag(DebuggerState.BreakPoint));

                // Check event

                Assert.IsTrue(bp1 && bp2);
            }
        }

        [TestMethod]
        public void ParseTest()
        {
            // Do the job
            IReverseTemplate n = new NeoTemplate();
            IReverser reverser = n.CreateReverser();

            Assert.IsInstanceOfType(reverser, typeof(NeoReverser));

            NeoConfig ini = new NeoConfig(SmartContractSampleRaw)
            {
                TriggerType = ETriggerType.Application,
                BlockChainPath = null,
            };

            ReverseResult rs = null;
            Assert.IsTrue(reverser.TryParse(ini, ref rs));

            // Parse test
            Assert.IsTrue(rs.Instructions.Count == SmartContractSampleTxt.Length);
            Assert.IsTrue(rs.Instructions.LastOrDefault().Location.Offset == 0x0F1C);

            for (int x = 0; x < SmartContractSampleTxt.Length; x++)
            {
                string[] sp = SmartContractSampleTxt[x].Split(' ');

                if (int.TryParse(sp[1], out int ix))
                {
                    // Fix push bytes
                    sp[1] = "PUSHBYTES" + sp[1];
                }

                Assert.AreEqual(sp[0], rs.Instructions[x].Location.Offset.ToString("x2").PadLeft(4, '0').ToUpperInvariant());
                Assert.AreEqual(sp[1], rs.Instructions[x].OpCode.Name);
            }

            // Write test
            using (MemoryStream ms = new MemoryStream())
            {
                foreach (Instruction i in rs.Instructions)
                    i.Write(ms);

                Assert.IsTrue(ms.ToArray().SequenceEqual(SmartContractSampleRaw));
            }
        }

        #region Sample
        string[] SmartContractSampleTxt =
@"0000 PUSHBYTES1      [19]//begincode(0)
0002 NEWARRAY        []//(0)
0003 TOALTSTACK      []//(0)
0004 FROMALTSTACK    []//set param:0(0)
0005 DUP             []
0006 TOALTSTACK      []
0007 PUSH0           []//(0)
0008 PUSH2           []//(0)
0009 ROLL            []
000A SETITEM         []
000B FROMALTSTACK    []//set param:1(0)
000C DUP             []
000D TOALTSTACK      []
000E PUSH1           []//(0)
000F PUSH2           []//(0)
0010 ROLL            []
0011 SETITEM         []
0012 NOP             []
0013 NOP             []
0014 SYSCALL         [164E656F2E52756E74696D652E47657454726967676572]
002C PUSH0           []
002D NUMEQUAL        []
002E FROMALTSTACK    []
002F DUP             []
0030 TOALTSTACK      []
0031 PUSH4           []
0032 PUSH2           []
0033 ROLL            []
0034 SETITEM         []
0035 FROMALTSTACK    []
0036 DUP             []
0037 TOALTSTACK      []
0038 PUSH4           []
0039 PICKITEM        []
003A JMPIFNOT        [C500]
003D NOP             []
003E 20              [2F3CAA21D8289402F29609549A32EDA0615A37B7]
0053 ARRAYSIZE       []
0054 PUSHBYTES1      [14]
0056 NUMEQUAL        []
0057 FROMALTSTACK    []
0058 DUP             []
0059 TOALTSTACK      []
005A PUSH5           []
005B PUSH2           []
005C ROLL            []
005D SETITEM         []
005E FROMALTSTACK    []
005F DUP             []
0060 TOALTSTACK      []
0061 PUSH5           []
0062 PICKITEM        []
0063 JMPIFNOT        [3E00]
0066 NOP             []
0067 20              [2F3CAA21D8289402F29609549A32EDA0615A37B7]
007C NOP             []
007D SYSCALL         [184E656F2E52756E74696D652E436865636B5769746E657373]
0097 FROMALTSTACK    []
0098 DUP             []
0099 TOALTSTACK      []
009A PUSH6           []
009B PUSH2           []
009C ROLL            []
009D SETITEM         []
009E JMP             [0903]
00A1 20              [2F3CAA21D8289402F29609549A32EDA0615A37B7]
00B6 ARRAYSIZE       []
00B7 PUSHBYTES1      [21]
00B9 NUMEQUAL        []
00BA FROMALTSTACK    []
00BB DUP             []
00BC TOALTSTACK      []
00BD PUSH7           []
00BE PUSH2           []
00BF ROLL            []
00C0 SETITEM         []
00C1 FROMALTSTACK    []
00C2 DUP             []
00C3 TOALTSTACK      []
00C4 PUSH7           []
00C5 PICKITEM        []
00C6 JMPIFNOT        [3500]
00C9 NOP             []
00CA FROMALTSTACK    []
00CB DUP             []
00CC TOALTSTACK      []
00CD PUSH0           []
00CE PICKITEM        []
00CF FROMALTSTACK    []
00D0 DUP             []
00D1 TOALTSTACK      []
00D2 PUSH8           []
00D3 PUSH2           []
00D4 ROLL            []
00D5 SETITEM         []
00D6 FROMALTSTACK    []
00D7 DUP             []
00D8 TOALTSTACK      []
00D9 PUSH8           []
00DA PICKITEM        []
00DB 20              [2F3CAA21D8289402F29609549A32EDA0615A37B7]
00F0 CHECKSIG        []
00F1 FROMALTSTACK    []
00F2 DUP             []
00F3 TOALTSTACK      []
00F4 PUSH6           []
00F5 PUSH2           []
00F6 ROLL            []
00F7 SETITEM         []
00F8 JMP             [AF02]
00FB NOP             []
00FC JMP             [3302]
00FF NOP             []
0100 SYSCALL         [164E656F2E52756E74696D652E47657454726967676572]
0118 PUSH16          []
0119 NUMEQUAL        []
011A FROMALTSTACK    []
011B DUP             []
011C TOALTSTACK      []
011D PUSH9           []
011E PUSH2           []
011F ROLL            []
0120 SETITEM         []
0121 FROMALTSTACK    []
0122 DUP             []
0123 TOALTSTACK      []
0124 PUSH9           []
0125 PICKITEM        []
0126 JMPIFNOT        [0902]
0129 NOP             []
012A FROMALTSTACK    []
012B DUP             []
012C TOALTSTACK      []
012D PUSH0           []
012E PICKITEM        []
012F 6               [6465706C6F79]
0136 EQUAL           []
0137 FROMALTSTACK    []
0138 DUP             []
0139 TOALTSTACK      []
013A PUSH10          []
013B PUSH2           []
013C ROLL            []
013D SETITEM         []
013E FROMALTSTACK    []
013F DUP             []
0140 TOALTSTACK      []
0141 PUSH10          []
0142 PICKITEM        []
0143 JMPIFNOT        [1100]
0146 NOP             []
0147 CALL            [9102]
014A FROMALTSTACK    []
014B DUP             []
014C TOALTSTACK      []
014D PUSH6           []
014E PUSH2           []
014F ROLL            []
0150 SETITEM         []
0151 JMP             [5602]
0154 FROMALTSTACK    []
0155 DUP             []
0156 TOALTSTACK      []
0157 PUSH0           []
0158 PICKITEM        []
0159 10              [6D696E74546F6B656E73]
0164 EQUAL           []
0165 FROMALTSTACK    []
0166 DUP             []
0167 TOALTSTACK      []
0168 PUSH11          []
0169 PUSH2           []
016A ROLL            []
016B SETITEM         []
016C FROMALTSTACK    []
016D DUP             []
016E TOALTSTACK      []
016F PUSH11          []
0170 PICKITEM        []
0171 JMPIFNOT        [1100]
0174 NOP             []
0175 CALL            [AA03]
0178 FROMALTSTACK    []
0179 DUP             []
017A TOALTSTACK      []
017B PUSH6           []
017C PUSH2           []
017D ROLL            []
017E SETITEM         []
017F JMP             [2802]
0182 FROMALTSTACK    []
0183 DUP             []
0184 TOALTSTACK      []
0185 PUSH0           []
0186 PICKITEM        []
0187 11              [746F74616C537570706C79]
0193 EQUAL           []
0194 FROMALTSTACK    []
0195 DUP             []
0196 TOALTSTACK      []
0197 PUSH12          []
0198 PUSH2           []
0199 ROLL            []
019A SETITEM         []
019B FROMALTSTACK    []
019C DUP             []
019D TOALTSTACK      []
019E PUSH12          []
019F PICKITEM        []
01A0 JMPIFNOT        [1100]
01A3 NOP             []
01A4 CALL            [8905]
01A7 FROMALTSTACK    []
01A8 DUP             []
01A9 TOALTSTACK      []
01AA PUSH6           []
01AB PUSH2           []
01AC ROLL            []
01AD SETITEM         []
01AE JMP             [F901]
01B1 FROMALTSTACK    []
01B2 DUP             []
01B3 TOALTSTACK      []
01B4 PUSH0           []
01B5 PICKITEM        []
01B6 4               [6E616D65]
01BB EQUAL           []
01BC FROMALTSTACK    []
01BD DUP             []
01BE TOALTSTACK      []
01BF PUSH13          []
01C0 PUSH2           []
01C1 ROLL            []
01C2 SETITEM         []
01C3 FROMALTSTACK    []
01C4 DUP             []
01C5 TOALTSTACK      []
01C6 PUSH13          []
01C7 PICKITEM        []
01C8 JMPIFNOT        [1100]
01CB NOP             []
01CC CALL            [E401]
01CF FROMALTSTACK    []
01D0 DUP             []
01D1 TOALTSTACK      []
01D2 PUSH6           []
01D3 PUSH2           []
01D4 ROLL            []
01D5 SETITEM         []
01D6 JMP             [D101]
01D9 FROMALTSTACK    []
01DA DUP             []
01DB TOALTSTACK      []
01DC PUSH0           []
01DD PICKITEM        []
01DE 6               [73796D626F6C]
01E5 EQUAL           []
01E6 FROMALTSTACK    []
01E7 DUP             []
01E8 TOALTSTACK      []
01E9 PUSH14          []
01EA PUSH2           []
01EB ROLL            []
01EC SETITEM         []
01ED FROMALTSTACK    []
01EE DUP             []
01EF TOALTSTACK      []
01F0 PUSH14          []
01F1 PICKITEM        []
01F2 JMPIFNOT        [1100]
01F5 NOP             []
01F6 CALL            [CF01]
01F9 FROMALTSTACK    []
01FA DUP             []
01FB TOALTSTACK      []
01FC PUSH6           []
01FD PUSH2           []
01FE ROLL            []
01FF SETITEM         []
0200 JMP             [A701]
0203 FROMALTSTACK    []
0204 DUP             []
0205 TOALTSTACK      []
0206 PUSH0           []
0207 PICKITEM        []
0208 8               [7472616E73666572]
0211 EQUAL           []
0212 FROMALTSTACK    []
0213 DUP             []
0214 TOALTSTACK      []
0215 PUSH15          []
0216 PUSH2           []
0217 ROLL            []
0218 SETITEM         []
0219 FROMALTSTACK    []
021A DUP             []
021B TOALTSTACK      []
021C PUSH15          []
021D PICKITEM        []
021E JMPIFNOT        [7700]
0221 NOP             []
0222 FROMALTSTACK    []
0223 DUP             []
0224 TOALTSTACK      []
0225 PUSH1           []
0226 PICKITEM        []
0227 ARRAYSIZE       []
0228 PUSH3           []
0229 NUMEQUAL        []
022A PUSH0           []
022B NUMEQUAL        []
022C FROMALTSTACK    []
022D DUP             []
022E TOALTSTACK      []
022F PUSHBYTES1      [13]
0231 PUSH2           []
0232 ROLL            []
0233 SETITEM         []
0234 FROMALTSTACK    []
0235 DUP             []
0236 TOALTSTACK      []
0237 PUSHBYTES1      [13]
0239 PICKITEM        []
023A JMPIFNOT        [0E00]
023D PUSH0           []
023E FROMALTSTACK    []
023F DUP             []
0240 TOALTSTACK      []
0241 PUSH6           []
0242 PUSH2           []
0243 ROLL            []
0244 SETITEM         []
0245 JMP             [6201]
0248 FROMALTSTACK    []
0249 DUP             []
024A TOALTSTACK      []
024B PUSH1           []
024C PICKITEM        []
024D PUSH0           []
024E PICKITEM        []
024F FROMALTSTACK    []
0250 DUP             []
0251 TOALTSTACK      []
0252 PUSH16          []
0253 PUSH2           []
0254 ROLL            []
0255 SETITEM         []
0256 FROMALTSTACK    []
0257 DUP             []
0258 TOALTSTACK      []
0259 PUSH1           []
025A PICKITEM        []
025B PUSH1           []
025C PICKITEM        []
025D FROMALTSTACK    []
025E DUP             []
025F TOALTSTACK      []
0260 PUSHBYTES1      [11]
0262 PUSH2           []
0263 ROLL            []
0264 SETITEM         []
0265 FROMALTSTACK    []
0266 DUP             []
0267 TOALTSTACK      []
0268 PUSH1           []
0269 PICKITEM        []
026A PUSH2           []
026B PICKITEM        []
026C FROMALTSTACK    []
026D DUP             []
026E TOALTSTACK      []
026F PUSHBYTES1      [12]
0271 PUSH2           []
0272 ROLL            []
0273 SETITEM         []
0274 FROMALTSTACK    []
0275 DUP             []
0276 TOALTSTACK      []
0277 PUSH16          []
0278 PICKITEM        []
0279 FROMALTSTACK    []
027A DUP             []
027B TOALTSTACK      []
027C PUSHBYTES1      [11]
027E PICKITEM        []
027F FROMALTSTACK    []
0280 DUP             []
0281 TOALTSTACK      []
0282 PUSHBYTES1      [12]
0284 PICKITEM        []
0285 NOP             []
0286 PUSH2           []//swap 0 and 2 param(0)
0287 XSWAP           []//(0)
0288 CALL            [F404]
028B FROMALTSTACK    []
028C DUP             []
028D TOALTSTACK      []
028E PUSH6           []
028F PUSH2           []
0290 ROLL            []
0291 SETITEM         []
0292 JMP             [1501]
0295 FROMALTSTACK    []
0296 DUP             []
0297 TOALTSTACK      []
0298 PUSH0           []
0299 PICKITEM        []
029A 9               [62616C616E63654F66]
02A4 EQUAL           []
02A5 FROMALTSTACK    []
02A6 DUP             []
02A7 TOALTSTACK      []
02A8 PUSHBYTES1      [14]
02AA PUSH2           []
02AB ROLL            []
02AC SETITEM         []
02AD FROMALTSTACK    []
02AE DUP             []
02AF TOALTSTACK      []
02B0 PUSHBYTES1      [14]
02B2 PICKITEM        []
02B3 JMPIFNOT        [4D00]
02B6 NOP             []
02B7 FROMALTSTACK    []
02B8 DUP             []
02B9 TOALTSTACK      []
02BA PUSH1           []
02BB PICKITEM        []
02BC ARRAYSIZE       []
02BD PUSH1           []
02BE NUMEQUAL        []
02BF PUSH0           []
02C0 NUMEQUAL        []
02C1 FROMALTSTACK    []
02C2 DUP             []
02C3 TOALTSTACK      []
02C4 PUSHBYTES1      [16]
02C6 PUSH2           []
02C7 ROLL            []
02C8 SETITEM         []
02C9 FROMALTSTACK    []
02CA DUP             []
02CB TOALTSTACK      []
02CC PUSHBYTES1      [16]
02CE PICKITEM        []
02CF JMPIFNOT        [0E00]
02D2 PUSH0           []
02D3 FROMALTSTACK    []
02D4 DUP             []
02D5 TOALTSTACK      []
02D6 PUSH6           []
02D7 PUSH2           []
02D8 ROLL            []
02D9 SETITEM         []
02DA JMP             [CD00]
02DD FROMALTSTACK    []
02DE DUP             []
02DF TOALTSTACK      []
02E0 PUSH1           []
02E1 PICKITEM        []
02E2 PUSH0           []
02E3 PICKITEM        []
02E4 FROMALTSTACK    []
02E5 DUP             []
02E6 TOALTSTACK      []
02E7 PUSHBYTES1      [15]
02E9 PUSH2           []
02EA ROLL            []
02EB SETITEM         []
02EC FROMALTSTACK    []
02ED DUP             []
02EE TOALTSTACK      []
02EF PUSHBYTES1      [15]
02F1 PICKITEM        []
02F2 NOP             []
02F3 CALL            [CD06]
02F6 FROMALTSTACK    []
02F7 DUP             []
02F8 TOALTSTACK      []
02F9 PUSH6           []
02FA PUSH2           []
02FB ROLL            []
02FC SETITEM         []
02FD JMP             [AA00]
0300 FROMALTSTACK    []
0301 DUP             []
0302 TOALTSTACK      []
0303 PUSH0           []
0304 PICKITEM        []
0305 8               [646563696D616C73]
030E EQUAL           []
030F FROMALTSTACK    []
0310 DUP             []
0311 TOALTSTACK      []
0312 PUSHBYTES1      [17]
0314 PUSH2           []
0315 ROLL            []
0316 SETITEM         []
0317 FROMALTSTACK    []
0318 DUP             []
0319 TOALTSTACK      []
031A PUSHBYTES1      [17]
031C PICKITEM        []
031D JMPIFNOT        [1100]
0320 NOP             []
0321 CALL            [AF00]
0324 FROMALTSTACK    []
0325 DUP             []
0326 TOALTSTACK      []
0327 PUSH6           []
0328 PUSH2           []
0329 ROLL            []
032A SETITEM         []
032B JMP             [7C00]
032E NOP             []
032F NOP             []
0330 CALL            [FD08]
0333 FROMALTSTACK    []
0334 DUP             []
0335 TOALTSTACK      []
0336 PUSH2           []
0337 PUSH2           []
0338 ROLL            []
0339 SETITEM         []
033A NOP             []
033B CALL            [710A]
033E FROMALTSTACK    []
033F DUP             []
0340 TOALTSTACK      []
0341 PUSH3           []
0342 PUSH2           []
0343 ROLL            []
0344 SETITEM         []
0345 FROMALTSTACK    []
0346 DUP             []
0347 TOALTSTACK      []
0348 PUSH3           []
0349 PICKITEM        []
034A PUSH0           []
034B ABS             []
034C SWAP            []
034D ABS             []
034E SWAP            []
034F LTE             []
0350 JMPIF           [0E00]
0353 FROMALTSTACK    []
0354 DUP             []
0355 TOALTSTACK      []
0356 PUSH2           []
0357 PICKITEM        []
0358 ARRAYSIZE       []
0359 PUSH0           []
035A GT              []
035B JMP             [0400]
035E PUSH0           []
035F FROMALTSTACK    []
0360 DUP             []
0361 TOALTSTACK      []
0362 PUSHBYTES1      [18]
0364 PUSH2           []
0365 ROLL            []
0366 SETITEM         []
0367 FROMALTSTACK    []
0368 DUP             []
0369 TOALTSTACK      []
036A PUSHBYTES1      [18]
036C PICKITEM        []
036D JMPIFNOT        [2F00]
0370 NOP             []
0371 FROMALTSTACK    []
0372 DUP             []
0373 TOALTSTACK      []
0374 PUSH2           []
0375 PICKITEM        []
0376 FROMALTSTACK    []
0377 DUP             []
0378 TOALTSTACK      []
0379 PUSH3           []
037A PICKITEM        []
037B NOP             []
037C SWAP            []//swap 2 param(0)
037D 6               [726566756E64]
0384 PUSH3           []
0385 PACK            []
0386 SYSCALL         [124E656F2E52756E74696D652E4E6F74696679]
039A NOP             []
039B NOP             []
039C PUSH0           []
039D FROMALTSTACK    []
039E DUP             []
039F TOALTSTACK      []
03A0 PUSH6           []
03A1 PUSH2           []
03A2 ROLL            []
03A3 SETITEM         []
03A4 JMP             [0300]
03A7 FROMALTSTACK    []
03A8 DUP             []
03A9 TOALTSTACK      []
03AA PUSH6           []
03AB PICKITEM        []
03AC NOP             []
03AD FROMALTSTACK    []//endcode(0)
03AE DROP            []//(0)
03AF RET             []
03B0 PUSH0           []//begincode(0)
03B1 NEWARRAY        []//(0)
03B2 TOALTSTACK      []//(0)
03B3 13              [526564345365632D546F6B656E]
03C1 NOP             []
03C2 FROMALTSTACK    []//endcode(0)
03C3 DROP            []//(0)
03C4 RET             []
03C5 PUSH0           []//begincode(0)
03C6 NEWARRAY        []//(0)
03C7 TOALTSTACK      []//(0)
03C8 3               [523453]
03CC NOP             []
03CD FROMALTSTACK    []//endcode(0)
03CE DROP            []//(0)
03CF RET             []
03D0 PUSH0           []//begincode(0)
03D1 NEWARRAY        []//(0)
03D2 TOALTSTACK      []//(0)
03D3 PUSH8           []
03D4 NOP             []
03D5 FROMALTSTACK    []//endcode(0)
03D6 DROP            []//(0)
03D7 RET             []
03D8 PUSH3           []//begincode(0)
03D9 NEWARRAY        []//(0)
03DA TOALTSTACK      []//(0)
03DB NOP             []
03DC NOP             []
03DD SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
03F5 11              [746F74616C537570706C79]
0401 NOP             []
0402 SWAP            []//swap 2 param(0)
0403 SYSCALL         [0F4E656F2E53746F726167652E476574]
0414 FROMALTSTACK    []
0415 DUP             []
0416 TOALTSTACK      []
0417 PUSH0           []
0418 PUSH2           []
0419 ROLL            []
041A SETITEM         []
041B FROMALTSTACK    []
041C DUP             []
041D TOALTSTACK      []
041E PUSH0           []
041F PICKITEM        []
0420 ARRAYSIZE       []
0421 PUSH0           []
0422 GT              []
0423 FROMALTSTACK    []
0424 DUP             []
0425 TOALTSTACK      []
0426 PUSH1           []
0427 PUSH2           []
0428 ROLL            []
0429 SETITEM         []
042A FROMALTSTACK    []
042B DUP             []
042C TOALTSTACK      []
042D PUSH1           []
042E PICKITEM        []
042F JMPIFNOT        [0E00]
0432 PUSH0           []
0433 FROMALTSTACK    []
0434 DUP             []
0435 TOALTSTACK      []
0436 PUSH2           []
0437 PUSH2           []
0438 ROLL            []
0439 SETITEM         []
043A JMP             [DC00]
043D NOP             []
043E SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
0456 20              [2F3CAA21D8289402F29609549A32EDA0615A37B7]
046B 7               [008053EE7BA80A]
0473 NOP             []
0474 PUSH2           []//swap 0 and 2 param(0)
0475 XSWAP           []//(0)
0476 SYSCALL         [0F4E656F2E53746F726167652E507574]
0487 NOP             []
0488 NOP             []
0489 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
04A1 11              [746F74616C537570706C79]
04AD 7               [008053EE7BA80A]
04B5 NOP             []
04B6 PUSH2           []//swap 0 and 2 param(0)
04B7 XSWAP           []//(0)
04B8 SYSCALL         [0F4E656F2E53746F726167652E507574]
04C9 NOP             []
04CA PUSH0           []
04CB 20              [2F3CAA21D8289402F29609549A32EDA0615A37B7]
04E0 7               [008053EE7BA80A]
04E8 NOP             []
04E9 PUSH2           []//swap 0 and 2 param(0)
04EA XSWAP           []//(0)
04EB 8               [7472616E73666572]
04F4 PUSH4           []
04F5 PACK            []
04F6 SYSCALL         [124E656F2E52756E74696D652E4E6F74696679]
050A NOP             []
050B PUSH1           []
050C FROMALTSTACK    []
050D DUP             []
050E TOALTSTACK      []
050F PUSH2           []
0510 PUSH2           []
0511 ROLL            []
0512 SETITEM         []
0513 JMP             [0300]
0516 FROMALTSTACK    []
0517 DUP             []
0518 TOALTSTACK      []
0519 PUSH2           []
051A PICKITEM        []
051B NOP             []
051C FROMALTSTACK    []//endcode(0)
051D DROP            []//(0)
051E RET             []
051F PUSH10          []//begincode(0)
0520 NEWARRAY        []//(0)
0521 TOALTSTACK      []//(0)
0522 NOP             []
0523 NOP             []
0524 CALL            [0907]
0527 FROMALTSTACK    []
0528 DUP             []
0529 TOALTSTACK      []
052A PUSH0           []
052B PUSH2           []
052C ROLL            []
052D SETITEM         []
052E FROMALTSTACK    []
052F DUP             []
0530 TOALTSTACK      []
0531 PUSH0           []
0532 PICKITEM        []
0533 ARRAYSIZE       []
0534 PUSH0           []
0535 NUMEQUAL        []
0536 FROMALTSTACK    []
0537 DUP             []
0538 TOALTSTACK      []
0539 PUSH6           []
053A PUSH2           []
053B ROLL            []
053C SETITEM         []
053D FROMALTSTACK    []
053E DUP             []
053F TOALTSTACK      []
0540 PUSH6           []
0541 PICKITEM        []
0542 JMPIFNOT        [0F00]
0545 NOP             []
0546 PUSH0           []
0547 FROMALTSTACK    []
0548 DUP             []
0549 TOALTSTACK      []
054A PUSH7           []
054B PUSH2           []
054C ROLL            []
054D SETITEM         []
054E JMP             [D601]
0551 NOP             []
0552 CALL            [5A08]
0555 FROMALTSTACK    []
0556 DUP             []
0557 TOALTSTACK      []
0558 PUSH1           []
0559 PUSH2           []
055A ROLL            []
055B SETITEM         []
055C NOP             []
055D CALL            [B204]
0560 FROMALTSTACK    []
0561 DUP             []
0562 TOALTSTACK      []
0563 PUSH2           []
0564 PUSH2           []
0565 ROLL            []
0566 SETITEM         []
0567 FROMALTSTACK    []
0568 DUP             []
0569 TOALTSTACK      []
056A PUSH2           []
056B PICKITEM        []
056C PUSH0           []
056D NUMEQUAL        []
056E FROMALTSTACK    []
056F DUP             []
0570 TOALTSTACK      []
0571 PUSH8           []
0572 PUSH2           []
0573 ROLL            []
0574 SETITEM         []
0575 FROMALTSTACK    []
0576 DUP             []
0577 TOALTSTACK      []
0578 PUSH8           []
0579 PICKITEM        []
057A JMPIFNOT        [3900]
057D NOP             []
057E FROMALTSTACK    []
057F DUP             []
0580 TOALTSTACK      []
0581 PUSH0           []
0582 PICKITEM        []
0583 FROMALTSTACK    []
0584 DUP             []
0585 TOALTSTACK      []
0586 PUSH1           []
0587 PICKITEM        []
0588 NOP             []
0589 SWAP            []//swap 2 param(0)
058A 6               [726566756E64]
0591 PUSH3           []
0592 PACK            []
0593 SYSCALL         [124E656F2E52756E74696D652E4E6F74696679]
05A7 NOP             []
05A8 PUSH0           []
05A9 FROMALTSTACK    []
05AA DUP             []
05AB TOALTSTACK      []
05AC PUSH7           []
05AD PUSH2           []
05AE ROLL            []
05AF SETITEM         []
05B0 JMP             [7401]
05B3 FROMALTSTACK    []
05B4 DUP             []
05B5 TOALTSTACK      []
05B6 PUSH0           []
05B7 PICKITEM        []
05B8 FROMALTSTACK    []
05B9 DUP             []
05BA TOALTSTACK      []
05BB PUSH1           []
05BC PICKITEM        []
05BD FROMALTSTACK    []
05BE DUP             []
05BF TOALTSTACK      []
05C0 PUSH2           []
05C1 PICKITEM        []
05C2 NOP             []
05C3 PUSH2           []//swap 0 and 2 param(0)
05C4 XSWAP           []//(0)
05C5 CALL            [1B05]
05C8 FROMALTSTACK    []
05C9 DUP             []
05CA TOALTSTACK      []
05CB PUSH3           []
05CC PUSH2           []
05CD ROLL            []
05CE SETITEM         []
05CF FROMALTSTACK    []
05D0 DUP             []
05D1 TOALTSTACK      []
05D2 PUSH3           []
05D3 PICKITEM        []
05D4 PUSH0           []
05D5 NUMEQUAL        []
05D6 FROMALTSTACK    []
05D7 DUP             []
05D8 TOALTSTACK      []
05D9 PUSH9           []
05DA PUSH2           []
05DB ROLL            []
05DC SETITEM         []
05DD FROMALTSTACK    []
05DE DUP             []
05DF TOALTSTACK      []
05E0 PUSH9           []
05E1 PICKITEM        []
05E2 JMPIFNOT        [0F00]
05E5 NOP             []
05E6 PUSH0           []
05E7 FROMALTSTACK    []
05E8 DUP             []
05E9 TOALTSTACK      []
05EA PUSH7           []
05EB PUSH2           []
05EC ROLL            []
05ED SETITEM         []
05EE JMP             [3601]
05F1 NOP             []
05F2 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
060A FROMALTSTACK    []
060B DUP             []
060C TOALTSTACK      []
060D PUSH0           []
060E PICKITEM        []
060F NOP             []
0610 SWAP            []//swap 2 param(0)
0611 SYSCALL         [0F4E656F2E53746F726167652E476574]
0622 FROMALTSTACK    []
0623 DUP             []
0624 TOALTSTACK      []
0625 PUSH4           []
0626 PUSH2           []
0627 ROLL            []
0628 SETITEM         []
0629 NOP             []
062A SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
0642 FROMALTSTACK    []
0643 DUP             []
0644 TOALTSTACK      []
0645 PUSH0           []
0646 PICKITEM        []
0647 FROMALTSTACK    []
0648 DUP             []
0649 TOALTSTACK      []
064A PUSH3           []
064B PICKITEM        []
064C FROMALTSTACK    []
064D DUP             []
064E TOALTSTACK      []
064F PUSH4           []
0650 PICKITEM        []
0651 ADD             []
0652 NOP             []
0653 PUSH2           []//swap 0 and 2 param(0)
0654 XSWAP           []//(0)
0655 SYSCALL         [0F4E656F2E53746F726167652E507574]
0666 NOP             []
0667 NOP             []
0668 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
0680 11              [746F74616C537570706C79]
068C NOP             []
068D SWAP            []//swap 2 param(0)
068E SYSCALL         [0F4E656F2E53746F726167652E476574]
069F FROMALTSTACK    []
06A0 DUP             []
06A1 TOALTSTACK      []
06A2 PUSH5           []
06A3 PUSH2           []
06A4 ROLL            []
06A5 SETITEM         []
06A6 NOP             []
06A7 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
06BF 11              [746F74616C537570706C79]
06CB FROMALTSTACK    []
06CC DUP             []
06CD TOALTSTACK      []
06CE PUSH3           []
06CF PICKITEM        []
06D0 FROMALTSTACK    []
06D1 DUP             []
06D2 TOALTSTACK      []
06D3 PUSH5           []
06D4 PICKITEM        []
06D5 ADD             []
06D6 NOP             []
06D7 PUSH2           []//swap 0 and 2 param(0)
06D8 XSWAP           []//(0)
06D9 SYSCALL         [0F4E656F2E53746F726167652E507574]
06EA NOP             []
06EB PUSH0           []
06EC FROMALTSTACK    []
06ED DUP             []
06EE TOALTSTACK      []
06EF PUSH0           []
06F0 PICKITEM        []
06F1 FROMALTSTACK    []
06F2 DUP             []
06F3 TOALTSTACK      []
06F4 PUSH3           []
06F5 PICKITEM        []
06F6 NOP             []
06F7 PUSH2           []//swap 0 and 2 param(0)
06F8 XSWAP           []//(0)
06F9 8               [7472616E73666572]
0702 PUSH4           []
0703 PACK            []
0704 SYSCALL         [124E656F2E52756E74696D652E4E6F74696679]
0718 NOP             []
0719 PUSH1           []
071A FROMALTSTACK    []
071B DUP             []
071C TOALTSTACK      []
071D PUSH7           []
071E PUSH2           []
071F ROLL            []
0720 SETITEM         []
0721 JMP             [0300]
0724 FROMALTSTACK    []
0725 DUP             []
0726 TOALTSTACK      []
0727 PUSH7           []
0728 PICKITEM        []
0729 NOP             []
072A FROMALTSTACK    []//endcode(0)
072B DROP            []//(0)
072C RET             []
072D PUSH1           []//begincode(0)
072E NEWARRAY        []//(0)
072F TOALTSTACK      []//(0)
0730 NOP             []
0731 NOP             []
0732 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
074A 11              [746F74616C537570706C79]
0756 NOP             []
0757 SWAP            []//swap 2 param(0)
0758 SYSCALL         [0F4E656F2E53746F726167652E476574]
0769 FROMALTSTACK    []
076A DUP             []
076B TOALTSTACK      []
076C PUSH0           []
076D PUSH2           []
076E ROLL            []
076F SETITEM         []
0770 JMP             [0300]
0773 FROMALTSTACK    []
0774 DUP             []
0775 TOALTSTACK      []
0776 PUSH0           []
0777 PICKITEM        []
0778 NOP             []
0779 FROMALTSTACK    []//endcode(0)
077A DROP            []//(0)
077B RET             []
077C PUSH11          []//begincode(0)
077D NEWARRAY        []//(0)
077E TOALTSTACK      []//(0)
077F FROMALTSTACK    []//set param:0(0)
0780 DUP             []
0781 TOALTSTACK      []
0782 PUSH0           []//(0)
0783 PUSH2           []//(0)
0784 ROLL            []
0785 SETITEM         []
0786 FROMALTSTACK    []//set param:1(0)
0787 DUP             []
0788 TOALTSTACK      []
0789 PUSH1           []//(0)
078A PUSH2           []//(0)
078B ROLL            []
078C SETITEM         []
078D FROMALTSTACK    []//set param:2(0)
078E DUP             []
078F TOALTSTACK      []
0790 PUSH2           []//(0)
0791 PUSH2           []//(0)
0792 ROLL            []
0793 SETITEM         []
0794 NOP             []
0795 FROMALTSTACK    []
0796 DUP             []
0797 TOALTSTACK      []
0798 PUSH2           []
0799 PICKITEM        []
079A PUSH0           []
079B LTE             []
079C FROMALTSTACK    []
079D DUP             []
079E TOALTSTACK      []
079F PUSH5           []
07A0 PUSH2           []
07A1 ROLL            []
07A2 SETITEM         []
07A3 FROMALTSTACK    []
07A4 DUP             []
07A5 TOALTSTACK      []
07A6 PUSH5           []
07A7 PICKITEM        []
07A8 JMPIFNOT        [0E00]
07AB PUSH0           []
07AC FROMALTSTACK    []
07AD DUP             []
07AE TOALTSTACK      []
07AF PUSH6           []
07B0 PUSH2           []
07B1 ROLL            []
07B2 SETITEM         []
07B3 JMP             [0402]
07B6 FROMALTSTACK    []
07B7 DUP             []
07B8 TOALTSTACK      []
07B9 PUSH0           []
07BA PICKITEM        []
07BB NOP             []
07BC SYSCALL         [184E656F2E52756E74696D652E436865636B5769746E657373]
07D6 PUSH0           []
07D7 NUMEQUAL        []
07D8 FROMALTSTACK    []
07D9 DUP             []
07DA TOALTSTACK      []
07DB PUSH7           []
07DC PUSH2           []
07DD ROLL            []
07DE SETITEM         []
07DF FROMALTSTACK    []
07E0 DUP             []
07E1 TOALTSTACK      []
07E2 PUSH7           []
07E3 PICKITEM        []
07E4 JMPIFNOT        [0E00]
07E7 PUSH0           []
07E8 FROMALTSTACK    []
07E9 DUP             []
07EA TOALTSTACK      []
07EB PUSH6           []
07EC PUSH2           []
07ED ROLL            []
07EE SETITEM         []
07EF JMP             [C801]
07F2 FROMALTSTACK    []
07F3 DUP             []
07F4 TOALTSTACK      []
07F5 PUSH0           []
07F6 PICKITEM        []
07F7 FROMALTSTACK    []
07F8 DUP             []
07F9 TOALTSTACK      []
07FA PUSH1           []
07FB PICKITEM        []
07FC NUMEQUAL        []
07FD FROMALTSTACK    []
07FE DUP             []
07FF TOALTSTACK      []
0800 PUSH8           []
0801 PUSH2           []
0802 ROLL            []
0803 SETITEM         []
0804 FROMALTSTACK    []
0805 DUP             []
0806 TOALTSTACK      []
0807 PUSH8           []
0808 PICKITEM        []
0809 JMPIFNOT        [0E00]
080C PUSH1           []
080D FROMALTSTACK    []
080E DUP             []
080F TOALTSTACK      []
0810 PUSH6           []
0811 PUSH2           []
0812 ROLL            []
0813 SETITEM         []
0814 JMP             [A301]
0817 NOP             []
0818 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
0830 FROMALTSTACK    []
0831 DUP             []
0832 TOALTSTACK      []
0833 PUSH0           []
0834 PICKITEM        []
0835 NOP             []
0836 SWAP            []//swap 2 param(0)
0837 SYSCALL         [0F4E656F2E53746F726167652E476574]
0848 FROMALTSTACK    []
0849 DUP             []
084A TOALTSTACK      []
084B PUSH3           []
084C PUSH2           []
084D ROLL            []
084E SETITEM         []
084F FROMALTSTACK    []
0850 DUP             []
0851 TOALTSTACK      []
0852 PUSH3           []
0853 PICKITEM        []
0854 FROMALTSTACK    []
0855 DUP             []
0856 TOALTSTACK      []
0857 PUSH2           []
0858 PICKITEM        []
0859 LT              []
085A FROMALTSTACK    []
085B DUP             []
085C TOALTSTACK      []
085D PUSH9           []
085E PUSH2           []
085F ROLL            []
0860 SETITEM         []
0861 FROMALTSTACK    []
0862 DUP             []
0863 TOALTSTACK      []
0864 PUSH9           []
0865 PICKITEM        []
0866 JMPIFNOT        [0E00]
0869 PUSH0           []
086A FROMALTSTACK    []
086B DUP             []
086C TOALTSTACK      []
086D PUSH6           []
086E PUSH2           []
086F ROLL            []
0870 SETITEM         []
0871 JMP             [4601]
0874 FROMALTSTACK    []
0875 DUP             []
0876 TOALTSTACK      []
0877 PUSH3           []
0878 PICKITEM        []
0879 FROMALTSTACK    []
087A DUP             []
087B TOALTSTACK      []
087C PUSH2           []
087D PICKITEM        []
087E NUMEQUAL        []
087F FROMALTSTACK    []
0880 DUP             []
0881 TOALTSTACK      []
0882 PUSH10          []
0883 PUSH2           []
0884 ROLL            []
0885 SETITEM         []
0886 FROMALTSTACK    []
0887 DUP             []
0888 TOALTSTACK      []
0889 PUSH10          []
088A PICKITEM        []
088B JMPIFNOT        [3B00]
088E NOP             []
088F SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
08A7 FROMALTSTACK    []
08A8 DUP             []
08A9 TOALTSTACK      []
08AA PUSH0           []
08AB PICKITEM        []
08AC NOP             []
08AD SWAP            []//swap 2 param(0)
08AE SYSCALL         [124E656F2E53746F726167652E44656C657465]
08C2 NOP             []
08C3 JMP             [4100]
08C6 NOP             []
08C7 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
08DF FROMALTSTACK    []
08E0 DUP             []
08E1 TOALTSTACK      []
08E2 PUSH0           []
08E3 PICKITEM        []
08E4 FROMALTSTACK    []
08E5 DUP             []
08E6 TOALTSTACK      []
08E7 PUSH3           []
08E8 PICKITEM        []
08E9 FROMALTSTACK    []
08EA DUP             []
08EB TOALTSTACK      []
08EC PUSH2           []
08ED PICKITEM        []
08EE SUB             []
08EF NOP             []
08F0 PUSH2           []//swap 0 and 2 param(0)
08F1 XSWAP           []//(0)
08F2 SYSCALL         [0F4E656F2E53746F726167652E507574]
0903 NOP             []
0904 NOP             []
0905 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
091D FROMALTSTACK    []
091E DUP             []
091F TOALTSTACK      []
0920 PUSH1           []
0921 PICKITEM        []
0922 NOP             []
0923 SWAP            []//swap 2 param(0)
0924 SYSCALL         [0F4E656F2E53746F726167652E476574]
0935 FROMALTSTACK    []
0936 DUP             []
0937 TOALTSTACK      []
0938 PUSH4           []
0939 PUSH2           []
093A ROLL            []
093B SETITEM         []
093C NOP             []
093D SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
0955 FROMALTSTACK    []
0956 DUP             []
0957 TOALTSTACK      []
0958 PUSH1           []
0959 PICKITEM        []
095A FROMALTSTACK    []
095B DUP             []
095C TOALTSTACK      []
095D PUSH4           []
095E PICKITEM        []
095F FROMALTSTACK    []
0960 DUP             []
0961 TOALTSTACK      []
0962 PUSH2           []
0963 PICKITEM        []
0964 ADD             []
0965 NOP             []
0966 PUSH2           []//swap 0 and 2 param(0)
0967 XSWAP           []//(0)
0968 SYSCALL         [0F4E656F2E53746F726167652E507574]
0979 NOP             []
097A FROMALTSTACK    []
097B DUP             []
097C TOALTSTACK      []
097D PUSH0           []
097E PICKITEM        []
097F FROMALTSTACK    []
0980 DUP             []
0981 TOALTSTACK      []
0982 PUSH1           []
0983 PICKITEM        []
0984 FROMALTSTACK    []
0985 DUP             []
0986 TOALTSTACK      []
0987 PUSH2           []
0988 PICKITEM        []
0989 NOP             []
098A PUSH2           []//swap 0 and 2 param(0)
098B XSWAP           []//(0)
098C 8               [7472616E73666572]
0995 PUSH4           []
0996 PACK            []
0997 SYSCALL         [124E656F2E52756E74696D652E4E6F74696679]
09AB NOP             []
09AC PUSH1           []
09AD FROMALTSTACK    []
09AE DUP             []
09AF TOALTSTACK      []
09B0 PUSH6           []
09B1 PUSH2           []
09B2 ROLL            []
09B3 SETITEM         []
09B4 JMP             [0300]
09B7 FROMALTSTACK    []
09B8 DUP             []
09B9 TOALTSTACK      []
09BA PUSH6           []
09BB PICKITEM        []
09BC NOP             []
09BD FROMALTSTACK    []//endcode(0)
09BE DROP            []//(0)
09BF RET             []
09C0 PUSH2           []//begincode(0)
09C1 NEWARRAY        []//(0)
09C2 TOALTSTACK      []//(0)
09C3 FROMALTSTACK    []//set param:0(0)
09C4 DUP             []
09C5 TOALTSTACK      []
09C6 PUSH0           []//(0)
09C7 PUSH2           []//(0)
09C8 ROLL            []
09C9 SETITEM         []
09CA NOP             []
09CB NOP             []
09CC SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
09E4 FROMALTSTACK    []
09E5 DUP             []
09E6 TOALTSTACK      []
09E7 PUSH0           []
09E8 PICKITEM        []
09E9 NOP             []
09EA SWAP            []//swap 2 param(0)
09EB SYSCALL         [0F4E656F2E53746F726167652E476574]
09FC FROMALTSTACK    []
09FD DUP             []
09FE TOALTSTACK      []
09FF PUSH1           []
0A00 PUSH2           []
0A01 ROLL            []
0A02 SETITEM         []
0A03 JMP             [0300]
0A06 FROMALTSTACK    []
0A07 DUP             []
0A08 TOALTSTACK      []
0A09 PUSH1           []
0A0A PICKITEM        []
0A0B NOP             []
0A0C FROMALTSTACK    []//endcode(0)
0A0D DROP            []//(0)
0A0E RET             []
0A0F PUSH5           []//begincode(0)
0A10 NEWARRAY        []//(0)
0A11 TOALTSTACK      []//(0)
0A12 NOP             []
0A13 NOP             []
0A14 SYSCALL         [184E656F2E426C6F636B636861696E2E476574486569676874]
0A2E NOP             []
0A2F SYSCALL         [184E656F2E426C6F636B636861696E2E476574486561646572]
0A49 NOP             []
0A4A SYSCALL         [174E656F2E4865616465722E47657454696D657374616D70]
0A63 PUSH15          []
0A64 ADD             []
0A65 FROMALTSTACK    []
0A66 DUP             []
0A67 TOALTSTACK      []
0A68 PUSH0           []
0A69 PUSH2           []
0A6A ROLL            []
0A6B SETITEM         []
0A6C FROMALTSTACK    []
0A6D DUP             []
0A6E TOALTSTACK      []
0A6F PUSH0           []
0A70 PICKITEM        []
0A71 4               [80BFCF59]
0A76 SUB             []
0A77 FROMALTSTACK    []
0A78 DUP             []
0A79 TOALTSTACK      []
0A7A PUSH1           []
0A7B PUSH2           []
0A7C ROLL            []
0A7D SETITEM         []
0A7E FROMALTSTACK    []
0A7F DUP             []
0A80 TOALTSTACK      []
0A81 PUSH1           []
0A82 PICKITEM        []
0A83 PUSH0           []
0A84 LT              []
0A85 FROMALTSTACK    []
0A86 DUP             []
0A87 TOALTSTACK      []
0A88 PUSH2           []
0A89 PUSH2           []
0A8A ROLL            []
0A8B SETITEM         []
0A8C FROMALTSTACK    []
0A8D DUP             []
0A8E TOALTSTACK      []
0A8F PUSH2           []
0A90 PICKITEM        []
0A91 JMPIFNOT        [0F00]
0A94 NOP             []
0A95 PUSH0           []
0A96 FROMALTSTACK    []
0A97 DUP             []
0A98 TOALTSTACK      []
0A99 PUSH3           []
0A9A PUSH2           []
0A9B ROLL            []
0A9C SETITEM         []
0A9D JMP             [3A00]
0AA0 FROMALTSTACK    []
0AA1 DUP             []
0AA2 TOALTSTACK      []
0AA3 PUSH1           []
0AA4 PICKITEM        []
0AA5 4               [8033E101]
0AAA LT              []
0AAB FROMALTSTACK    []
0AAC DUP             []
0AAD TOALTSTACK      []
0AAE PUSH4           []
0AAF PUSH2           []
0AB0 ROLL            []
0AB1 SETITEM         []
0AB2 FROMALTSTACK    []
0AB3 DUP             []
0AB4 TOALTSTACK      []
0AB5 PUSH4           []
0AB6 PICKITEM        []
0AB7 JMPIFNOT        [1400]
0ABA NOP             []
0ABB 5               [00E8764817]
0AC1 FROMALTSTACK    []
0AC2 DUP             []
0AC3 TOALTSTACK      []
0AC4 PUSH3           []
0AC5 PUSH2           []
0AC6 ROLL            []
0AC7 SETITEM         []
0AC8 JMP             [0F00]
0ACB NOP             []
0ACC PUSH0           []
0ACD FROMALTSTACK    []
0ACE DUP             []
0ACF TOALTSTACK      []
0AD0 PUSH3           []
0AD1 PUSH2           []
0AD2 ROLL            []
0AD3 SETITEM         []
0AD4 JMP             [0300]
0AD7 FROMALTSTACK    []
0AD8 DUP             []
0AD9 TOALTSTACK      []
0ADA PUSH3           []
0ADB PICKITEM        []
0ADC NOP             []
0ADD FROMALTSTACK    []//endcode(0)
0ADE DROP            []//(0)
0ADF RET             []
0AE0 PUSH9           []//begincode(0)
0AE1 NEWARRAY        []//(0)
0AE2 TOALTSTACK      []//(0)
0AE3 FROMALTSTACK    []//set param:0(0)
0AE4 DUP             []
0AE5 TOALTSTACK      []
0AE6 PUSH0           []//(0)
0AE7 PUSH2           []//(0)
0AE8 ROLL            []
0AE9 SETITEM         []
0AEA FROMALTSTACK    []//set param:1(0)
0AEB DUP             []
0AEC TOALTSTACK      []
0AED PUSH1           []//(0)
0AEE PUSH2           []//(0)
0AEF ROLL            []
0AF0 SETITEM         []
0AF1 FROMALTSTACK    []//set param:2(0)
0AF2 DUP             []
0AF3 TOALTSTACK      []
0AF4 PUSH2           []//(0)
0AF5 PUSH2           []//(0)
0AF6 ROLL            []
0AF7 SETITEM         []
0AF8 NOP             []
0AF9 FROMALTSTACK    []
0AFA DUP             []
0AFB TOALTSTACK      []
0AFC PUSH1           []
0AFD PICKITEM        []
0AFE 4               [00E1F505]
0B03 DIV             []
0B04 FROMALTSTACK    []
0B05 DUP             []
0B06 TOALTSTACK      []
0B07 PUSH2           []
0B08 PICKITEM        []
0B09 MUL             []
0B0A FROMALTSTACK    []
0B0B DUP             []
0B0C TOALTSTACK      []
0B0D PUSH3           []
0B0E PUSH2           []
0B0F ROLL            []
0B10 SETITEM         []
0B11 NOP             []
0B12 SYSCALL         [164E656F2E53746F726167652E476574436F6E74657874]
0B2A 11              [746F74616C537570706C79]
0B36 NOP             []
0B37 SWAP            []//swap 2 param(0)
0B38 SYSCALL         [0F4E656F2E53746F726167652E476574]
0B49 FROMALTSTACK    []
0B4A DUP             []
0B4B TOALTSTACK      []
0B4C PUSH4           []
0B4D PUSH2           []
0B4E ROLL            []
0B4F SETITEM         []
0B50 7               [0000C16FF28623]
0B58 FROMALTSTACK    []
0B59 DUP             []
0B5A TOALTSTACK      []
0B5B PUSH4           []
0B5C PICKITEM        []
0B5D SUB             []
0B5E FROMALTSTACK    []
0B5F DUP             []
0B60 TOALTSTACK      []
0B61 PUSH5           []
0B62 PUSH2           []
0B63 ROLL            []
0B64 SETITEM         []
0B65 FROMALTSTACK    []
0B66 DUP             []
0B67 TOALTSTACK      []
0B68 PUSH5           []
0B69 PICKITEM        []
0B6A PUSH0           []
0B6B LTE             []
0B6C FROMALTSTACK    []
0B6D DUP             []
0B6E TOALTSTACK      []
0B6F PUSH6           []
0B70 PUSH2           []
0B71 ROLL            []
0B72 SETITEM         []
0B73 FROMALTSTACK    []
0B74 DUP             []
0B75 TOALTSTACK      []
0B76 PUSH6           []
0B77 PICKITEM        []
0B78 JMPIFNOT        [3900]
0B7B NOP             []
0B7C FROMALTSTACK    []
0B7D DUP             []
0B7E TOALTSTACK      []
0B7F PUSH0           []
0B80 PICKITEM        []
0B81 FROMALTSTACK    []
0B82 DUP             []
0B83 TOALTSTACK      []
0B84 PUSH1           []
0B85 PICKITEM        []
0B86 NOP             []
0B87 SWAP            []//swap 2 param(0)
0B88 6               [726566756E64]
0B8F PUSH3           []
0B90 PACK            []
0B91 SYSCALL         [124E656F2E52756E74696D652E4E6F74696679]
0BA5 NOP             []
0BA6 PUSH0           []
0BA7 FROMALTSTACK    []
0BA8 DUP             []
0BA9 TOALTSTACK      []
0BAA PUSH7           []
0BAB PUSH2           []
0BAC ROLL            []
0BAD SETITEM         []
0BAE JMP             [7600]
0BB1 FROMALTSTACK    []
0BB2 DUP             []
0BB3 TOALTSTACK      []
0BB4 PUSH5           []
0BB5 PICKITEM        []
0BB6 FROMALTSTACK    []
0BB7 DUP             []
0BB8 TOALTSTACK      []
0BB9 PUSH3           []
0BBA PICKITEM        []
0BBB LT              []
0BBC FROMALTSTACK    []
0BBD DUP             []
0BBE TOALTSTACK      []
0BBF PUSH8           []
0BC0 PUSH2           []
0BC1 ROLL            []
0BC2 SETITEM         []
0BC3 FROMALTSTACK    []
0BC4 DUP             []
0BC5 TOALTSTACK      []
0BC6 PUSH8           []
0BC7 PICKITEM        []
0BC8 JMPIFNOT        [4D00]
0BCB NOP             []
0BCC FROMALTSTACK    []
0BCD DUP             []
0BCE TOALTSTACK      []
0BCF PUSH0           []
0BD0 PICKITEM        []
0BD1 FROMALTSTACK    []
0BD2 DUP             []
0BD3 TOALTSTACK      []
0BD4 PUSH3           []
0BD5 PICKITEM        []
0BD6 FROMALTSTACK    []
0BD7 DUP             []
0BD8 TOALTSTACK      []
0BD9 PUSH5           []
0BDA PICKITEM        []
0BDB SUB             []
0BDC FROMALTSTACK    []
0BDD DUP             []
0BDE TOALTSTACK      []
0BDF PUSH2           []
0BE0 PICKITEM        []
0BE1 DIV             []
0BE2 4               [00E1F505]
0BE7 MUL             []
0BE8 NOP             []
0BE9 SWAP            []//swap 2 param(0)
0BEA 6               [726566756E64]
0BF1 PUSH3           []
0BF2 PACK            []
0BF3 SYSCALL         [124E656F2E52756E74696D652E4E6F74696679]
0C07 NOP             []
0C08 FROMALTSTACK    []
0C09 DUP             []
0C0A TOALTSTACK      []
0C0B PUSH5           []
0C0C PICKITEM        []
0C0D FROMALTSTACK    []
0C0E DUP             []
0C0F TOALTSTACK      []
0C10 PUSH3           []
0C11 PUSH2           []
0C12 ROLL            []
0C13 SETITEM         []
0C14 NOP             []
0C15 FROMALTSTACK    []
0C16 DUP             []
0C17 TOALTSTACK      []
0C18 PUSH3           []
0C19 PICKITEM        []
0C1A FROMALTSTACK    []
0C1B DUP             []
0C1C TOALTSTACK      []
0C1D PUSH7           []
0C1E PUSH2           []
0C1F ROLL            []
0C20 SETITEM         []
0C21 JMP             [0300]
0C24 FROMALTSTACK    []
0C25 DUP             []
0C26 TOALTSTACK      []
0C27 PUSH7           []
0C28 PICKITEM        []
0C29 NOP             []
0C2A FROMALTSTACK    []//endcode(0)
0C2B DROP            []//(0)
0C2C RET             []
0C2D PUSH7           []//begincode(0)
0C2E NEWARRAY        []//(0)
0C2F TOALTSTACK      []//(0)
0C30 NOP             []
0C31 NOP             []
0C32 SYSCALL         [2953797374656D2E457865637574696F6E456E67696E652E476574536372697074436F6E7461696E6572]
0C5D FROMALTSTACK    []
0C5E DUP             []
0C5F TOALTSTACK      []
0C60 PUSH0           []
0C61 PUSH2           []
0C62 ROLL            []
0C63 SETITEM         []
0C64 FROMALTSTACK    []
0C65 DUP             []
0C66 TOALTSTACK      []
0C67 PUSH0           []
0C68 PICKITEM        []
0C69 NOP             []
0C6A SYSCALL         [1D4E656F2E5472616E73616374696F6E2E4765745265666572656E636573]
0C89 FROMALTSTACK    []
0C8A DUP             []
0C8B TOALTSTACK      []
0C8C PUSH1           []
0C8D PUSH2           []
0C8E ROLL            []
0C8F SETITEM         []
0C90 NOP             []
0C91 FROMALTSTACK    []
0C92 DUP             []
0C93 TOALTSTACK      []
0C94 PUSH1           []
0C95 PICKITEM        []
0C96 FROMALTSTACK    []
0C97 DUP             []
0C98 TOALTSTACK      []
0C99 PUSH2           []
0C9A PUSH2           []
0C9B ROLL            []
0C9C SETITEM         []
0C9D PUSH0           []
0C9E FROMALTSTACK    []
0C9F DUP             []
0CA0 TOALTSTACK      []
0CA1 PUSH3           []
0CA2 PUSH2           []
0CA3 ROLL            []
0CA4 SETITEM         []
0CA5 JMP             [9D00]
0CA8 FROMALTSTACK    []
0CA9 DUP             []
0CAA TOALTSTACK      []
0CAB PUSH2           []
0CAC PICKITEM        []
0CAD FROMALTSTACK    []
0CAE DUP             []
0CAF TOALTSTACK      []
0CB0 PUSH3           []
0CB1 PICKITEM        []
0CB2 PICKITEM        []
0CB3 FROMALTSTACK    []
0CB4 DUP             []
0CB5 TOALTSTACK      []
0CB6 PUSH4           []
0CB7 PUSH2           []
0CB8 ROLL            []
0CB9 SETITEM         []
0CBA NOP             []
0CBB FROMALTSTACK    []
0CBC DUP             []
0CBD TOALTSTACK      []
0CBE PUSH4           []
0CBF PICKITEM        []
0CC0 NOP             []
0CC1 SYSCALL         [154E656F2E4F75747075742E47657441737365744964]
0CD8 32              [9B7CFFDAA674BEAE0F930EBE6085AF9093E5FE56B34A5C220CCDCF6EFC336FC5]
0CF9 NUMEQUAL        []
0CFA FROMALTSTACK    []
0CFB DUP             []
0CFC TOALTSTACK      []
0CFD PUSH5           []
0CFE PUSH2           []
0CFF ROLL            []
0D00 SETITEM         []
0D01 FROMALTSTACK    []
0D02 DUP             []
0D03 TOALTSTACK      []
0D04 PUSH5           []
0D05 PICKITEM        []
0D06 JMPIFNOT        [2D00]
0D09 FROMALTSTACK    []
0D0A DUP             []
0D0B TOALTSTACK      []
0D0C PUSH4           []
0D0D PICKITEM        []
0D0E NOP             []
0D0F SYSCALL         [184E656F2E4F75747075742E47657453637269707448617368]
0D29 FROMALTSTACK    []
0D2A DUP             []
0D2B TOALTSTACK      []
0D2C PUSH6           []
0D2D PUSH2           []
0D2E ROLL            []
0D2F SETITEM         []
0D30 JMP             [2C00]
0D33 NOP             []
0D34 FROMALTSTACK    []
0D35 DUP             []
0D36 TOALTSTACK      []
0D37 PUSH3           []
0D38 PICKITEM        []
0D39 PUSH1           []
0D3A ADD             []
0D3B FROMALTSTACK    []
0D3C DUP             []
0D3D TOALTSTACK      []
0D3E PUSH3           []
0D3F PUSH2           []
0D40 ROLL            []
0D41 SETITEM         []
0D42 FROMALTSTACK    []
0D43 DUP             []
0D44 TOALTSTACK      []
0D45 PUSH3           []
0D46 PICKITEM        []
0D47 FROMALTSTACK    []
0D48 DUP             []
0D49 TOALTSTACK      []
0D4A PUSH2           []
0D4B PICKITEM        []
0D4C ARRAYSIZE       []
0D4D LT              []
0D4E JMPIF           [5AFF]
0D51 PUSH0           []
0D52 FROMALTSTACK    []
0D53 DUP             []
0D54 TOALTSTACK      []
0D55 PUSH6           []
0D56 PUSH2           []
0D57 ROLL            []
0D58 SETITEM         []
0D59 JMP             [0300]
0D5C FROMALTSTACK    []
0D5D DUP             []
0D5E TOALTSTACK      []
0D5F PUSH6           []
0D60 PICKITEM        []
0D61 NOP             []
0D62 FROMALTSTACK    []//endcode(0)
0D63 DROP            []//(0)
0D64 RET             []
0D65 PUSH1           []//begincode(0)
0D66 NEWARRAY        []//(0)
0D67 TOALTSTACK      []//(0)
0D68 NOP             []
0D69 NOP             []
0D6A SYSCALL         [2D53797374656D2E457865637574696F6E456E67696E652E476574457865637574696E6753637269707448617368]
0D99 FROMALTSTACK    []
0D9A DUP             []
0D9B TOALTSTACK      []
0D9C PUSH0           []
0D9D PUSH2           []
0D9E ROLL            []
0D9F SETITEM         []
0DA0 JMP             [0300]
0DA3 FROMALTSTACK    []
0DA4 DUP             []
0DA5 TOALTSTACK      []
0DA6 PUSH0           []
0DA7 PICKITEM        []
0DA8 NOP             []
0DA9 FROMALTSTACK    []//endcode(0)
0DAA DROP            []//(0)
0DAB RET             []
0DAC PUSH8           []//begincode(0)
0DAD NEWARRAY        []//(0)
0DAE TOALTSTACK      []//(0)
0DAF NOP             []
0DB0 NOP             []
0DB1 SYSCALL         [2953797374656D2E457865637574696F6E456E67696E652E476574536372697074436F6E7461696E6572]
0DDC FROMALTSTACK    []
0DDD DUP             []
0DDE TOALTSTACK      []
0DDF PUSH0           []
0DE0 PUSH2           []
0DE1 ROLL            []
0DE2 SETITEM         []
0DE3 FROMALTSTACK    []
0DE4 DUP             []
0DE5 TOALTSTACK      []
0DE6 PUSH0           []
0DE7 PICKITEM        []
0DE8 NOP             []
0DE9 SYSCALL         [1A4E656F2E5472616E73616374696F6E2E4765744F757470757473]
0E05 FROMALTSTACK    []
0E06 DUP             []
0E07 TOALTSTACK      []
0E08 PUSH1           []
0E09 PUSH2           []
0E0A ROLL            []
0E0B SETITEM         []
0E0C PUSH0           []
0E0D FROMALTSTACK    []
0E0E DUP             []
0E0F TOALTSTACK      []
0E10 PUSH2           []
0E11 PUSH2           []
0E12 ROLL            []
0E13 SETITEM         []
0E14 NOP             []
0E15 FROMALTSTACK    []
0E16 DUP             []
0E17 TOALTSTACK      []
0E18 PUSH1           []
0E19 PICKITEM        []
0E1A FROMALTSTACK    []
0E1B DUP             []
0E1C TOALTSTACK      []
0E1D PUSH3           []
0E1E PUSH2           []
0E1F ROLL            []
0E20 SETITEM         []
0E21 PUSH0           []
0E22 FROMALTSTACK    []
0E23 DUP             []
0E24 TOALTSTACK      []
0E25 PUSH4           []
0E26 PUSH2           []
0E27 ROLL            []
0E28 SETITEM         []
0E29 JMP             [CD00]
0E2C FROMALTSTACK    []
0E2D DUP             []
0E2E TOALTSTACK      []
0E2F PUSH3           []
0E30 PICKITEM        []
0E31 FROMALTSTACK    []
0E32 DUP             []
0E33 TOALTSTACK      []
0E34 PUSH4           []
0E35 PICKITEM        []
0E36 PICKITEM        []
0E37 FROMALTSTACK    []
0E38 DUP             []
0E39 TOALTSTACK      []
0E3A PUSH5           []
0E3B PUSH2           []
0E3C ROLL            []
0E3D SETITEM         []
0E3E NOP             []
0E3F FROMALTSTACK    []
0E40 DUP             []
0E41 TOALTSTACK      []
0E42 PUSH5           []
0E43 PICKITEM        []
0E44 NOP             []
0E45 SYSCALL         [184E656F2E4F75747075742E47657453637269707448617368]
0E5F NOP             []
0E60 CALL            [05FF]
0E63 ABS             []
0E64 SWAP            []
0E65 ABS             []
0E66 SWAP            []
0E67 NUMNOTEQUAL     []
0E68 JMPIF           [4500]
0E6B FROMALTSTACK    []
0E6C DUP             []
0E6D TOALTSTACK      []
0E6E PUSH5           []
0E6F PICKITEM        []
0E70 NOP             []
0E71 SYSCALL         [154E656F2E4F75747075742E47657441737365744964]
0E88 32              [9B7CFFDAA674BEAE0F930EBE6085AF9093E5FE56B34A5C220CCDCF6EFC336FC5]
0EA9 NUMEQUAL        []
0EAA JMP             [0400]
0EAD PUSH0           []
0EAE FROMALTSTACK    []
0EAF DUP             []
0EB0 TOALTSTACK      []
0EB1 PUSH6           []
0EB2 PUSH2           []
0EB3 ROLL            []
0EB4 SETITEM         []
0EB5 FROMALTSTACK    []
0EB6 DUP             []
0EB7 TOALTSTACK      []
0EB8 PUSH6           []
0EB9 PICKITEM        []
0EBA JMPIFNOT        [2D00]
0EBD NOP             []
0EBE FROMALTSTACK    []
0EBF DUP             []
0EC0 TOALTSTACK      []
0EC1 PUSH2           []
0EC2 PICKITEM        []
0EC3 FROMALTSTACK    []
0EC4 DUP             []
0EC5 TOALTSTACK      []
0EC6 PUSH5           []
0EC7 PICKITEM        []
0EC8 NOP             []
0EC9 SYSCALL         [134E656F2E4F75747075742E47657456616C7565]
0EDE ADD             []
0EDF FROMALTSTACK    []
0EE0 DUP             []
0EE1 TOALTSTACK      []
0EE2 PUSH2           []
0EE3 PUSH2           []
0EE4 ROLL            []
0EE5 SETITEM         []
0EE6 NOP             []
0EE7 NOP             []
0EE8 FROMALTSTACK    []
0EE9 DUP             []
0EEA TOALTSTACK      []
0EEB PUSH4           []
0EEC PICKITEM        []
0EED PUSH1           []
0EEE ADD             []
0EEF FROMALTSTACK    []
0EF0 DUP             []
0EF1 TOALTSTACK      []
0EF2 PUSH4           []
0EF3 PUSH2           []
0EF4 ROLL            []
0EF5 SETITEM         []
0EF6 FROMALTSTACK    []
0EF7 DUP             []
0EF8 TOALTSTACK      []
0EF9 PUSH4           []
0EFA PICKITEM        []
0EFB FROMALTSTACK    []
0EFC DUP             []
0EFD TOALTSTACK      []
0EFE PUSH3           []
0EFF PICKITEM        []
0F00 ARRAYSIZE       []
0F01 LT              []
0F02 JMPIF           [2AFF]
0F05 FROMALTSTACK    []
0F06 DUP             []
0F07 TOALTSTACK      []
0F08 PUSH2           []
0F09 PICKITEM        []
0F0A FROMALTSTACK    []
0F0B DUP             []
0F0C TOALTSTACK      []
0F0D PUSH7           []
0F0E PUSH2           []
0F0F ROLL            []
0F10 SETITEM         []
0F11 JMP             [0300]
0F14 FROMALTSTACK    []
0F15 DUP             []
0F16 TOALTSTACK      []
0F17 PUSH7           []
0F18 PICKITEM        []
0F19 NOP             []
0F1A FROMALTSTACK    []//endcode(0)
0F1B DROP            []//(0)
0F1C RET             []".Trim().Split('\n');
        byte[] SmartContractSampleRaw =
            {
    0x01, 0x19, 0xC5, 0x6B, 0x6C, 0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x6C,
    0x76, 0x6B, 0x51, 0x52, 0x7A, 0xC4, 0x61, 0x61, 0x68, 0x16, 0x4E, 0x65,
    0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x47, 0x65,
    0x74, 0x54, 0x72, 0x69, 0x67, 0x67, 0x65, 0x72, 0x00, 0x9C, 0x6C, 0x76,
    0x6B, 0x54, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x64, 0xC5,
    0x00, 0x61, 0x14, 0x2F, 0x3C, 0xAA, 0x21, 0xD8, 0x28, 0x94, 0x02, 0xF2,
    0x96, 0x09, 0x54, 0x9A, 0x32, 0xED, 0xA0, 0x61, 0x5A, 0x37, 0xB7, 0xC0,
    0x01, 0x14, 0x9C, 0x6C, 0x76, 0x6B, 0x55, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x55, 0xC3, 0x64, 0x3E, 0x00, 0x61, 0x14, 0x2F, 0x3C, 0xAA, 0x21,
    0xD8, 0x28, 0x94, 0x02, 0xF2, 0x96, 0x09, 0x54, 0x9A, 0x32, 0xED, 0xA0,
    0x61, 0x5A, 0x37, 0xB7, 0x61, 0x68, 0x18, 0x4E, 0x65, 0x6F, 0x2E, 0x52,
    0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x43, 0x68, 0x65, 0x63, 0x6B,
    0x57, 0x69, 0x74, 0x6E, 0x65, 0x73, 0x73, 0x6C, 0x76, 0x6B, 0x56, 0x52,
    0x7A, 0xC4, 0x62, 0x09, 0x03, 0x14, 0x2F, 0x3C, 0xAA, 0x21, 0xD8, 0x28,
    0x94, 0x02, 0xF2, 0x96, 0x09, 0x54, 0x9A, 0x32, 0xED, 0xA0, 0x61, 0x5A,
    0x37, 0xB7, 0xC0, 0x01, 0x21, 0x9C, 0x6C, 0x76, 0x6B, 0x57, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x57, 0xC3, 0x64, 0x35, 0x00, 0x61, 0x6C, 0x76,
    0x6B, 0x00, 0xC3, 0x6C, 0x76, 0x6B, 0x58, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x58, 0xC3, 0x14, 0x2F, 0x3C, 0xAA, 0x21, 0xD8, 0x28, 0x94, 0x02,
    0xF2, 0x96, 0x09, 0x54, 0x9A, 0x32, 0xED, 0xA0, 0x61, 0x5A, 0x37, 0xB7,
    0xAC, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0xAF, 0x02, 0x61,
    0x62, 0x33, 0x02, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75,
    0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x54, 0x72, 0x69,
    0x67, 0x67, 0x65, 0x72, 0x60, 0x9C, 0x6C, 0x76, 0x6B, 0x59, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x59, 0xC3, 0x64, 0x09, 0x02, 0x61, 0x6C, 0x76,
    0x6B, 0x00, 0xC3, 0x06, 0x64, 0x65, 0x70, 0x6C, 0x6F, 0x79, 0x87, 0x6C,
    0x76, 0x6B, 0x5A, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x5A, 0xC3, 0x64,
    0x11, 0x00, 0x61, 0x65, 0x91, 0x02, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A,
    0xC4, 0x62, 0x56, 0x02, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x0A, 0x6D, 0x69,
    0x6E, 0x74, 0x54, 0x6F, 0x6B, 0x65, 0x6E, 0x73, 0x87, 0x6C, 0x76, 0x6B,
    0x5B, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x5B, 0xC3, 0x64, 0x11, 0x00,
    0x61, 0x65, 0xAA, 0x03, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62,
    0x28, 0x02, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x0B, 0x74, 0x6F, 0x74, 0x61,
    0x6C, 0x53, 0x75, 0x70, 0x70, 0x6C, 0x79, 0x87, 0x6C, 0x76, 0x6B, 0x5C,
    0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x5C, 0xC3, 0x64, 0x11, 0x00, 0x61,
    0x65, 0x89, 0x05, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0xF9,
    0x01, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x04, 0x6E, 0x61, 0x6D, 0x65, 0x87,
    0x6C, 0x76, 0x6B, 0x5D, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x5D, 0xC3,
    0x64, 0x11, 0x00, 0x61, 0x65, 0xE4, 0x01, 0x6C, 0x76, 0x6B, 0x56, 0x52,
    0x7A, 0xC4, 0x62, 0xD1, 0x01, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x06, 0x73,
    0x79, 0x6D, 0x62, 0x6F, 0x6C, 0x87, 0x6C, 0x76, 0x6B, 0x5E, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x5E, 0xC3, 0x64, 0x11, 0x00, 0x61, 0x65, 0xCF,
    0x01, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0xA7, 0x01, 0x6C,
    0x76, 0x6B, 0x00, 0xC3, 0x08, 0x74, 0x72, 0x61, 0x6E, 0x73, 0x66, 0x65,
    0x72, 0x87, 0x6C, 0x76, 0x6B, 0x5F, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B,
    0x5F, 0xC3, 0x64, 0x77, 0x00, 0x61, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0xC0,
    0x53, 0x9C, 0x00, 0x9C, 0x6C, 0x76, 0x6B, 0x01, 0x13, 0x52, 0x7A, 0xC4,
    0x6C, 0x76, 0x6B, 0x01, 0x13, 0xC3, 0x64, 0x0E, 0x00, 0x00, 0x6C, 0x76,
    0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0x62, 0x01, 0x6C, 0x76, 0x6B, 0x51,
    0xC3, 0x00, 0xC3, 0x6C, 0x76, 0x6B, 0x60, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x51, 0xC3, 0x51, 0xC3, 0x6C, 0x76, 0x6B, 0x01, 0x11, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x52, 0xC3, 0x6C, 0x76, 0x6B, 0x01,
    0x12, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x60, 0xC3, 0x6C, 0x76, 0x6B,
    0x01, 0x11, 0xC3, 0x6C, 0x76, 0x6B, 0x01, 0x12, 0xC3, 0x61, 0x52, 0x72,
    0x65, 0xF4, 0x04, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0x15,
    0x01, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x09, 0x62, 0x61, 0x6C, 0x61, 0x6E,
    0x63, 0x65, 0x4F, 0x66, 0x87, 0x6C, 0x76, 0x6B, 0x01, 0x14, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x01, 0x14, 0xC3, 0x64, 0x4D, 0x00, 0x61, 0x6C,
    0x76, 0x6B, 0x51, 0xC3, 0xC0, 0x51, 0x9C, 0x00, 0x9C, 0x6C, 0x76, 0x6B,
    0x01, 0x16, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x01, 0x16, 0xC3, 0x64,
    0x0E, 0x00, 0x00, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0xCD,
    0x00, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x00, 0xC3, 0x6C, 0x76, 0x6B, 0x01,
    0x15, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x01, 0x15, 0xC3, 0x61, 0x65,
    0xCD, 0x06, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0xAA, 0x00,
    0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x08, 0x64, 0x65, 0x63, 0x69, 0x6D, 0x61,
    0x6C, 0x73, 0x87, 0x6C, 0x76, 0x6B, 0x01, 0x17, 0x52, 0x7A, 0xC4, 0x6C,
    0x76, 0x6B, 0x01, 0x17, 0xC3, 0x64, 0x11, 0x00, 0x61, 0x65, 0xAF, 0x00,
    0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0x7C, 0x00, 0x61, 0x61,
    0x65, 0xFD, 0x08, 0x6C, 0x76, 0x6B, 0x52, 0x52, 0x7A, 0xC4, 0x61, 0x65,
    0x71, 0x0A, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B,
    0x53, 0xC3, 0x00, 0x90, 0x7C, 0x90, 0x7C, 0xA1, 0x63, 0x0E, 0x00, 0x6C,
    0x76, 0x6B, 0x52, 0xC3, 0xC0, 0x00, 0xA0, 0x62, 0x04, 0x00, 0x00, 0x6C,
    0x76, 0x6B, 0x01, 0x18, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x01, 0x18,
    0xC3, 0x64, 0x2F, 0x00, 0x61, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x6C, 0x76,
    0x6B, 0x53, 0xC3, 0x61, 0x7C, 0x06, 0x72, 0x65, 0x66, 0x75, 0x6E, 0x64,
    0x53, 0xC1, 0x68, 0x12, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74,
    0x69, 0x6D, 0x65, 0x2E, 0x4E, 0x6F, 0x74, 0x69, 0x66, 0x79, 0x61, 0x61,
    0x00, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00, 0x6C,
    0x76, 0x6B, 0x56, 0xC3, 0x61, 0x6C, 0x75, 0x66, 0x00, 0xC5, 0x6B, 0x0D,
    0x52, 0x65, 0x64, 0x34, 0x53, 0x65, 0x63, 0x2D, 0x54, 0x6F, 0x6B, 0x65,
    0x6E, 0x61, 0x6C, 0x75, 0x66, 0x00, 0xC5, 0x6B, 0x03, 0x52, 0x34, 0x53,
    0x61, 0x6C, 0x75, 0x66, 0x00, 0xC5, 0x6B, 0x58, 0x61, 0x6C, 0x75, 0x66,
    0x53, 0xC5, 0x6B, 0x61, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53,
    0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F,
    0x6E, 0x74, 0x65, 0x78, 0x74, 0x0B, 0x74, 0x6F, 0x74, 0x61, 0x6C, 0x53,
    0x75, 0x70, 0x70, 0x6C, 0x79, 0x61, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F,
    0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74,
    0x6C, 0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x00, 0xC3,
    0xC0, 0x00, 0xA0, 0x6C, 0x76, 0x6B, 0x51, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x51, 0xC3, 0x64, 0x0E, 0x00, 0x00, 0x6C, 0x76, 0x6B, 0x52, 0x52,
    0x7A, 0xC4, 0x62, 0xDC, 0x00, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E,
    0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43,
    0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x14, 0x2F, 0x3C, 0xAA, 0x21, 0xD8,
    0x28, 0x94, 0x02, 0xF2, 0x96, 0x09, 0x54, 0x9A, 0x32, 0xED, 0xA0, 0x61,
    0x5A, 0x37, 0xB7, 0x07, 0x00, 0x80, 0x53, 0xEE, 0x7B, 0xA8, 0x0A, 0x61,
    0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72,
    0x61, 0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61, 0x61, 0x68, 0x16, 0x4E,
    0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47,
    0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x0B, 0x74, 0x6F,
    0x74, 0x61, 0x6C, 0x53, 0x75, 0x70, 0x70, 0x6C, 0x79, 0x07, 0x00, 0x80,
    0x53, 0xEE, 0x7B, 0xA8, 0x0A, 0x61, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75,
    0x74, 0x61, 0x00, 0x14, 0x2F, 0x3C, 0xAA, 0x21, 0xD8, 0x28, 0x94, 0x02,
    0xF2, 0x96, 0x09, 0x54, 0x9A, 0x32, 0xED, 0xA0, 0x61, 0x5A, 0x37, 0xB7,
    0x07, 0x00, 0x80, 0x53, 0xEE, 0x7B, 0xA8, 0x0A, 0x61, 0x52, 0x72, 0x08,
    0x74, 0x72, 0x61, 0x6E, 0x73, 0x66, 0x65, 0x72, 0x54, 0xC1, 0x68, 0x12,
    0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E,
    0x4E, 0x6F, 0x74, 0x69, 0x66, 0x79, 0x61, 0x51, 0x6C, 0x76, 0x6B, 0x52,
    0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x61,
    0x6C, 0x75, 0x66, 0x5A, 0xC5, 0x6B, 0x61, 0x61, 0x65, 0x09, 0x07, 0x6C,
    0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0xC0,
    0x00, 0x9C, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B,
    0x56, 0xC3, 0x64, 0x0F, 0x00, 0x61, 0x00, 0x6C, 0x76, 0x6B, 0x57, 0x52,
    0x7A, 0xC4, 0x62, 0xD6, 0x01, 0x61, 0x65, 0x5A, 0x08, 0x6C, 0x76, 0x6B,
    0x51, 0x52, 0x7A, 0xC4, 0x61, 0x65, 0xB2, 0x04, 0x6C, 0x76, 0x6B, 0x52,
    0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x00, 0x9C, 0x6C, 0x76,
    0x6B, 0x58, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x58, 0xC3, 0x64, 0x39,
    0x00, 0x61, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x6C, 0x76, 0x6B, 0x51, 0xC3,
    0x61, 0x7C, 0x06, 0x72, 0x65, 0x66, 0x75, 0x6E, 0x64, 0x53, 0xC1, 0x68,
    0x12, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65,
    0x2E, 0x4E, 0x6F, 0x74, 0x69, 0x66, 0x79, 0x61, 0x00, 0x6C, 0x76, 0x6B,
    0x57, 0x52, 0x7A, 0xC4, 0x62, 0x74, 0x01, 0x6C, 0x76, 0x6B, 0x00, 0xC3,
    0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x61, 0x52,
    0x72, 0x65, 0x1B, 0x05, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A, 0xC4, 0x6C,
    0x76, 0x6B, 0x53, 0xC3, 0x00, 0x9C, 0x6C, 0x76, 0x6B, 0x59, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x59, 0xC3, 0x64, 0x0F, 0x00, 0x61, 0x00, 0x6C,
    0x76, 0x6B, 0x57, 0x52, 0x7A, 0xC4, 0x62, 0x36, 0x01, 0x61, 0x68, 0x16,
    0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E,
    0x47, 0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x6C, 0x76,
    0x6B, 0x00, 0xC3, 0x61, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53,
    0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x6C, 0x76,
    0x6B, 0x54, 0x52, 0x7A, 0xC4, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E,
    0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43,
    0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x6C,
    0x76, 0x6B, 0x53, 0xC3, 0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x93, 0x61, 0x52,
    0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61,
    0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61, 0x61, 0x68, 0x16, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65,
    0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x0B, 0x74, 0x6F, 0x74,
    0x61, 0x6C, 0x53, 0x75, 0x70, 0x70, 0x6C, 0x79, 0x61, 0x7C, 0x68, 0x0F,
    0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E,
    0x47, 0x65, 0x74, 0x6C, 0x76, 0x6B, 0x55, 0x52, 0x7A, 0xC4, 0x61, 0x68,
    0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65,
    0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x0B,
    0x74, 0x6F, 0x74, 0x61, 0x6C, 0x53, 0x75, 0x70, 0x70, 0x6C, 0x79, 0x6C,
    0x76, 0x6B, 0x53, 0xC3, 0x6C, 0x76, 0x6B, 0x55, 0xC3, 0x93, 0x61, 0x52,
    0x72, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61,
    0x67, 0x65, 0x2E, 0x50, 0x75, 0x74, 0x61, 0x00, 0x6C, 0x76, 0x6B, 0x00,
    0xC3, 0x6C, 0x76, 0x6B, 0x53, 0xC3, 0x61, 0x52, 0x72, 0x08, 0x74, 0x72,
    0x61, 0x6E, 0x73, 0x66, 0x65, 0x72, 0x54, 0xC1, 0x68, 0x12, 0x4E, 0x65,
    0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4E, 0x6F,
    0x74, 0x69, 0x66, 0x79, 0x61, 0x51, 0x6C, 0x76, 0x6B, 0x57, 0x52, 0x7A,
    0xC4, 0x62, 0x03, 0x00, 0x6C, 0x76, 0x6B, 0x57, 0xC3, 0x61, 0x6C, 0x75,
    0x66, 0x51, 0xC5, 0x6B, 0x61, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E,
    0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43,
    0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x0B, 0x74, 0x6F, 0x74, 0x61, 0x6C,
    0x53, 0x75, 0x70, 0x70, 0x6C, 0x79, 0x61, 0x7C, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65,
    0x74, 0x6C, 0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00, 0x6C,
    0x76, 0x6B, 0x00, 0xC3, 0x61, 0x6C, 0x75, 0x66, 0x5B, 0xC5, 0x6B, 0x6C,
    0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x51, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x52, 0x52, 0x7A, 0xC4, 0x61, 0x6C, 0x76, 0x6B,
    0x52, 0xC3, 0x00, 0xA1, 0x6C, 0x76, 0x6B, 0x55, 0x52, 0x7A, 0xC4, 0x6C,
    0x76, 0x6B, 0x55, 0xC3, 0x64, 0x0E, 0x00, 0x00, 0x6C, 0x76, 0x6B, 0x56,
    0x52, 0x7A, 0xC4, 0x62, 0x04, 0x02, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x61,
    0x68, 0x18, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D,
    0x65, 0x2E, 0x43, 0x68, 0x65, 0x63, 0x6B, 0x57, 0x69, 0x74, 0x6E, 0x65,
    0x73, 0x73, 0x00, 0x9C, 0x6C, 0x76, 0x6B, 0x57, 0x52, 0x7A, 0xC4, 0x6C,
    0x76, 0x6B, 0x57, 0xC3, 0x64, 0x0E, 0x00, 0x00, 0x6C, 0x76, 0x6B, 0x56,
    0x52, 0x7A, 0xC4, 0x62, 0xC8, 0x01, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x6C,
    0x76, 0x6B, 0x51, 0xC3, 0x9C, 0x6C, 0x76, 0x6B, 0x58, 0x52, 0x7A, 0xC4,
    0x6C, 0x76, 0x6B, 0x58, 0xC3, 0x64, 0x0E, 0x00, 0x51, 0x6C, 0x76, 0x6B,
    0x56, 0x52, 0x7A, 0xC4, 0x62, 0xA3, 0x01, 0x61, 0x68, 0x16, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65,
    0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x6C, 0x76, 0x6B, 0x00,
    0xC3, 0x61, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F,
    0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x6C, 0x76, 0x6B, 0x53,
    0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x53, 0xC3, 0x6C, 0x76, 0x6B, 0x52,
    0xC3, 0x9F, 0x6C, 0x76, 0x6B, 0x59, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B,
    0x59, 0xC3, 0x64, 0x0E, 0x00, 0x00, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A,
    0xC4, 0x62, 0x46, 0x01, 0x6C, 0x76, 0x6B, 0x53, 0xC3, 0x6C, 0x76, 0x6B,
    0x52, 0xC3, 0x9C, 0x6C, 0x76, 0x6B, 0x5A, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x5A, 0xC3, 0x64, 0x3B, 0x00, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F,
    0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74,
    0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74, 0x6C, 0x76, 0x6B, 0x00, 0xC3,
    0x61, 0x7C, 0x68, 0x12, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72,
    0x61, 0x67, 0x65, 0x2E, 0x44, 0x65, 0x6C, 0x65, 0x74, 0x65, 0x61, 0x62,
    0x41, 0x00, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F,
    0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74,
    0x65, 0x78, 0x74, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x6C, 0x76, 0x6B, 0x53,
    0xC3, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x94, 0x61, 0x52, 0x72, 0x68, 0x0F,
    0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E,
    0x50, 0x75, 0x74, 0x61, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53,
    0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F,
    0x6E, 0x74, 0x65, 0x78, 0x74, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x61, 0x7C,
    0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67,
    0x65, 0x2E, 0x47, 0x65, 0x74, 0x6C, 0x76, 0x6B, 0x54, 0x52, 0x7A, 0xC4,
    0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61,
    0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78,
    0x74, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x6C,
    0x76, 0x6B, 0x52, 0xC3, 0x93, 0x61, 0x52, 0x72, 0x68, 0x0F, 0x4E, 0x65,
    0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x50, 0x75,
    0x74, 0x61, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x6C, 0x76, 0x6B, 0x51, 0xC3,
    0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x61, 0x52, 0x72, 0x08, 0x74, 0x72, 0x61,
    0x6E, 0x73, 0x66, 0x65, 0x72, 0x54, 0xC1, 0x68, 0x12, 0x4E, 0x65, 0x6F,
    0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4E, 0x6F, 0x74,
    0x69, 0x66, 0x79, 0x61, 0x51, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4,
    0x62, 0x03, 0x00, 0x6C, 0x76, 0x6B, 0x56, 0xC3, 0x61, 0x6C, 0x75, 0x66,
    0x52, 0xC5, 0x6B, 0x6C, 0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x61, 0x61,
    0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67,
    0x65, 0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65, 0x78, 0x74,
    0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x61, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F,
    0x2E, 0x53, 0x74, 0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74,
    0x6C, 0x76, 0x6B, 0x51, 0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00, 0x6C, 0x76,
    0x6B, 0x51, 0xC3, 0x61, 0x6C, 0x75, 0x66, 0x55, 0xC5, 0x6B, 0x61, 0x61,
    0x68, 0x18, 0x4E, 0x65, 0x6F, 0x2E, 0x42, 0x6C, 0x6F, 0x63, 0x6B, 0x63,
    0x68, 0x61, 0x69, 0x6E, 0x2E, 0x47, 0x65, 0x74, 0x48, 0x65, 0x69, 0x67,
    0x68, 0x74, 0x61, 0x68, 0x18, 0x4E, 0x65, 0x6F, 0x2E, 0x42, 0x6C, 0x6F,
    0x63, 0x6B, 0x63, 0x68, 0x61, 0x69, 0x6E, 0x2E, 0x47, 0x65, 0x74, 0x48,
    0x65, 0x61, 0x64, 0x65, 0x72, 0x61, 0x68, 0x17, 0x4E, 0x65, 0x6F, 0x2E,
    0x48, 0x65, 0x61, 0x64, 0x65, 0x72, 0x2E, 0x47, 0x65, 0x74, 0x54, 0x69,
    0x6D, 0x65, 0x73, 0x74, 0x61, 0x6D, 0x70, 0x5F, 0x93, 0x6C, 0x76, 0x6B,
    0x00, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x04, 0x80, 0xBF,
    0xCF, 0x59, 0x94, 0x6C, 0x76, 0x6B, 0x51, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x51, 0xC3, 0x00, 0x9F, 0x6C, 0x76, 0x6B, 0x52, 0x52, 0x7A, 0xC4,
    0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x64, 0x0F, 0x00, 0x61, 0x00, 0x6C, 0x76,
    0x6B, 0x53, 0x52, 0x7A, 0xC4, 0x62, 0x3A, 0x00, 0x6C, 0x76, 0x6B, 0x51,
    0xC3, 0x04, 0x80, 0x33, 0xE1, 0x01, 0x9F, 0x6C, 0x76, 0x6B, 0x54, 0x52,
    0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x64, 0x14, 0x00, 0x61, 0x05,
    0x00, 0xE8, 0x76, 0x48, 0x17, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A, 0xC4,
    0x62, 0x0F, 0x00, 0x61, 0x00, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A, 0xC4,
    0x62, 0x03, 0x00, 0x6C, 0x76, 0x6B, 0x53, 0xC3, 0x61, 0x6C, 0x75, 0x66,
    0x59, 0xC5, 0x6B, 0x6C, 0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x51, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x52, 0x52, 0x7A, 0xC4,
    0x61, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x04, 0x00, 0xE1, 0xF5, 0x05, 0x96,
    0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x95, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A,
    0xC4, 0x61, 0x68, 0x16, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74, 0x6F, 0x72,
    0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x43, 0x6F, 0x6E, 0x74, 0x65,
    0x78, 0x74, 0x0B, 0x74, 0x6F, 0x74, 0x61, 0x6C, 0x53, 0x75, 0x70, 0x70,
    0x6C, 0x79, 0x61, 0x7C, 0x68, 0x0F, 0x4E, 0x65, 0x6F, 0x2E, 0x53, 0x74,
    0x6F, 0x72, 0x61, 0x67, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x6C, 0x76, 0x6B,
    0x54, 0x52, 0x7A, 0xC4, 0x07, 0x00, 0x00, 0xC1, 0x6F, 0xF2, 0x86, 0x23,
    0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x94, 0x6C, 0x76, 0x6B, 0x55, 0x52, 0x7A,
    0xC4, 0x6C, 0x76, 0x6B, 0x55, 0xC3, 0x00, 0xA1, 0x6C, 0x76, 0x6B, 0x56,
    0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x56, 0xC3, 0x64, 0x39, 0x00, 0x61,
    0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x61, 0x7C,
    0x06, 0x72, 0x65, 0x66, 0x75, 0x6E, 0x64, 0x53, 0xC1, 0x68, 0x12, 0x4E,
    0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65, 0x2E, 0x4E,
    0x6F, 0x74, 0x69, 0x66, 0x79, 0x61, 0x00, 0x6C, 0x76, 0x6B, 0x57, 0x52,
    0x7A, 0xC4, 0x62, 0x76, 0x00, 0x6C, 0x76, 0x6B, 0x55, 0xC3, 0x6C, 0x76,
    0x6B, 0x53, 0xC3, 0x9F, 0x6C, 0x76, 0x6B, 0x58, 0x52, 0x7A, 0xC4, 0x6C,
    0x76, 0x6B, 0x58, 0xC3, 0x64, 0x4D, 0x00, 0x61, 0x6C, 0x76, 0x6B, 0x00,
    0xC3, 0x6C, 0x76, 0x6B, 0x53, 0xC3, 0x6C, 0x76, 0x6B, 0x55, 0xC3, 0x94,
    0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x96, 0x04, 0x00, 0xE1, 0xF5, 0x05, 0x95,
    0x61, 0x7C, 0x06, 0x72, 0x65, 0x66, 0x75, 0x6E, 0x64, 0x53, 0xC1, 0x68,
    0x12, 0x4E, 0x65, 0x6F, 0x2E, 0x52, 0x75, 0x6E, 0x74, 0x69, 0x6D, 0x65,
    0x2E, 0x4E, 0x6F, 0x74, 0x69, 0x66, 0x79, 0x61, 0x6C, 0x76, 0x6B, 0x55,
    0xC3, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A, 0xC4, 0x61, 0x6C, 0x76, 0x6B,
    0x53, 0xC3, 0x6C, 0x76, 0x6B, 0x57, 0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00,
    0x6C, 0x76, 0x6B, 0x57, 0xC3, 0x61, 0x6C, 0x75, 0x66, 0x57, 0xC5, 0x6B,
    0x61, 0x61, 0x68, 0x29, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x45,
    0x78, 0x65, 0x63, 0x75, 0x74, 0x69, 0x6F, 0x6E, 0x45, 0x6E, 0x67, 0x69,
    0x6E, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x53, 0x63, 0x72, 0x69, 0x70, 0x74,
    0x43, 0x6F, 0x6E, 0x74, 0x61, 0x69, 0x6E, 0x65, 0x72, 0x6C, 0x76, 0x6B,
    0x00, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x61, 0x68, 0x1D,
    0x4E, 0x65, 0x6F, 0x2E, 0x54, 0x72, 0x61, 0x6E, 0x73, 0x61, 0x63, 0x74,
    0x69, 0x6F, 0x6E, 0x2E, 0x47, 0x65, 0x74, 0x52, 0x65, 0x66, 0x65, 0x72,
    0x65, 0x6E, 0x63, 0x65, 0x73, 0x6C, 0x76, 0x6B, 0x51, 0x52, 0x7A, 0xC4,
    0x61, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x6C, 0x76, 0x6B, 0x52, 0x52, 0x7A,
    0xC4, 0x00, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A, 0xC4, 0x62, 0x9D, 0x00,
    0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x6C, 0x76, 0x6B, 0x53, 0xC3, 0xC3, 0x6C,
    0x76, 0x6B, 0x54, 0x52, 0x7A, 0xC4, 0x61, 0x6C, 0x76, 0x6B, 0x54, 0xC3,
    0x61, 0x68, 0x15, 0x4E, 0x65, 0x6F, 0x2E, 0x4F, 0x75, 0x74, 0x70, 0x75,
    0x74, 0x2E, 0x47, 0x65, 0x74, 0x41, 0x73, 0x73, 0x65, 0x74, 0x49, 0x64,
    0x20, 0x9B, 0x7C, 0xFF, 0xDA, 0xA6, 0x74, 0xBE, 0xAE, 0x0F, 0x93, 0x0E,
    0xBE, 0x60, 0x85, 0xAF, 0x90, 0x93, 0xE5, 0xFE, 0x56, 0xB3, 0x4A, 0x5C,
    0x22, 0x0C, 0xCD, 0xCF, 0x6E, 0xFC, 0x33, 0x6F, 0xC5, 0x9C, 0x6C, 0x76,
    0x6B, 0x55, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x55, 0xC3, 0x64, 0x2D,
    0x00, 0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x61, 0x68, 0x18, 0x4E, 0x65, 0x6F,
    0x2E, 0x4F, 0x75, 0x74, 0x70, 0x75, 0x74, 0x2E, 0x47, 0x65, 0x74, 0x53,
    0x63, 0x72, 0x69, 0x70, 0x74, 0x48, 0x61, 0x73, 0x68, 0x6C, 0x76, 0x6B,
    0x56, 0x52, 0x7A, 0xC4, 0x62, 0x2C, 0x00, 0x61, 0x6C, 0x76, 0x6B, 0x53,
    0xC3, 0x51, 0x93, 0x6C, 0x76, 0x6B, 0x53, 0x52, 0x7A, 0xC4, 0x6C, 0x76,
    0x6B, 0x53, 0xC3, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0xC0, 0x9F, 0x63, 0x5A,
    0xFF, 0x00, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00,
    0x6C, 0x76, 0x6B, 0x56, 0xC3, 0x61, 0x6C, 0x75, 0x66, 0x51, 0xC5, 0x6B,
    0x61, 0x61, 0x68, 0x2D, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x45,
    0x78, 0x65, 0x63, 0x75, 0x74, 0x69, 0x6F, 0x6E, 0x45, 0x6E, 0x67, 0x69,
    0x6E, 0x65, 0x2E, 0x47, 0x65, 0x74, 0x45, 0x78, 0x65, 0x63, 0x75, 0x74,
    0x69, 0x6E, 0x67, 0x53, 0x63, 0x72, 0x69, 0x70, 0x74, 0x48, 0x61, 0x73,
    0x68, 0x6C, 0x76, 0x6B, 0x00, 0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00, 0x6C,
    0x76, 0x6B, 0x00, 0xC3, 0x61, 0x6C, 0x75, 0x66, 0x58, 0xC5, 0x6B, 0x61,
    0x61, 0x68, 0x29, 0x53, 0x79, 0x73, 0x74, 0x65, 0x6D, 0x2E, 0x45, 0x78,
    0x65, 0x63, 0x75, 0x74, 0x69, 0x6F, 0x6E, 0x45, 0x6E, 0x67, 0x69, 0x6E,
    0x65, 0x2E, 0x47, 0x65, 0x74, 0x53, 0x63, 0x72, 0x69, 0x70, 0x74, 0x43,
    0x6F, 0x6E, 0x74, 0x61, 0x69, 0x6E, 0x65, 0x72, 0x6C, 0x76, 0x6B, 0x00,
    0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x00, 0xC3, 0x61, 0x68, 0x1A, 0x4E,
    0x65, 0x6F, 0x2E, 0x54, 0x72, 0x61, 0x6E, 0x73, 0x61, 0x63, 0x74, 0x69,
    0x6F, 0x6E, 0x2E, 0x47, 0x65, 0x74, 0x4F, 0x75, 0x74, 0x70, 0x75, 0x74,
    0x73, 0x6C, 0x76, 0x6B, 0x51, 0x52, 0x7A, 0xC4, 0x00, 0x6C, 0x76, 0x6B,
    0x52, 0x52, 0x7A, 0xC4, 0x61, 0x6C, 0x76, 0x6B, 0x51, 0xC3, 0x6C, 0x76,
    0x6B, 0x53, 0x52, 0x7A, 0xC4, 0x00, 0x6C, 0x76, 0x6B, 0x54, 0x52, 0x7A,
    0xC4, 0x62, 0xCD, 0x00, 0x6C, 0x76, 0x6B, 0x53, 0xC3, 0x6C, 0x76, 0x6B,
    0x54, 0xC3, 0xC3, 0x6C, 0x76, 0x6B, 0x55, 0x52, 0x7A, 0xC4, 0x61, 0x6C,
    0x76, 0x6B, 0x55, 0xC3, 0x61, 0x68, 0x18, 0x4E, 0x65, 0x6F, 0x2E, 0x4F,
    0x75, 0x74, 0x70, 0x75, 0x74, 0x2E, 0x47, 0x65, 0x74, 0x53, 0x63, 0x72,
    0x69, 0x70, 0x74, 0x48, 0x61, 0x73, 0x68, 0x61, 0x65, 0x05, 0xFF, 0x90,
    0x7C, 0x90, 0x7C, 0x9E, 0x63, 0x45, 0x00, 0x6C, 0x76, 0x6B, 0x55, 0xC3,
    0x61, 0x68, 0x15, 0x4E, 0x65, 0x6F, 0x2E, 0x4F, 0x75, 0x74, 0x70, 0x75,
    0x74, 0x2E, 0x47, 0x65, 0x74, 0x41, 0x73, 0x73, 0x65, 0x74, 0x49, 0x64,
    0x20, 0x9B, 0x7C, 0xFF, 0xDA, 0xA6, 0x74, 0xBE, 0xAE, 0x0F, 0x93, 0x0E,
    0xBE, 0x60, 0x85, 0xAF, 0x90, 0x93, 0xE5, 0xFE, 0x56, 0xB3, 0x4A, 0x5C,
    0x22, 0x0C, 0xCD, 0xCF, 0x6E, 0xFC, 0x33, 0x6F, 0xC5, 0x9C, 0x62, 0x04,
    0x00, 0x00, 0x6C, 0x76, 0x6B, 0x56, 0x52, 0x7A, 0xC4, 0x6C, 0x76, 0x6B,
    0x56, 0xC3, 0x64, 0x2D, 0x00, 0x61, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x6C,
    0x76, 0x6B, 0x55, 0xC3, 0x61, 0x68, 0x13, 0x4E, 0x65, 0x6F, 0x2E, 0x4F,
    0x75, 0x74, 0x70, 0x75, 0x74, 0x2E, 0x47, 0x65, 0x74, 0x56, 0x61, 0x6C,
    0x75, 0x65, 0x93, 0x6C, 0x76, 0x6B, 0x52, 0x52, 0x7A, 0xC4, 0x61, 0x61,
    0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x51, 0x93, 0x6C, 0x76, 0x6B, 0x54, 0x52,
    0x7A, 0xC4, 0x6C, 0x76, 0x6B, 0x54, 0xC3, 0x6C, 0x76, 0x6B, 0x53, 0xC3,
    0xC0, 0x9F, 0x63, 0x2A, 0xFF, 0x6C, 0x76, 0x6B, 0x52, 0xC3, 0x6C, 0x76,
    0x6B, 0x57, 0x52, 0x7A, 0xC4, 0x62, 0x03, 0x00, 0x6C, 0x76, 0x6B, 0x57,
    0xC3, 0x61, 0x6C, 0x75, 0x66
};
        #endregion
    }
}
