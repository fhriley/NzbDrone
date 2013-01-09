﻿// ReSharper disable RedundantUsingDirective

using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using NzbDrone.Core.Repository.Quality;
using NzbDrone.Core.Test.Framework;

namespace NzbDrone.Core.Test.ParserTests
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    public class QualityParserFixture : CoreTest
    {
        public static object[] QualityParserCases =
        {
            new object[] { "WEEDS.S03E01-06.DUAL.BDRip.XviD.AC3.-HELLYWOOD", QualityTypes.DVD, false },
            new object[] { "WEEDS.S03E01-06.DUAL.BDRip.X-viD.AC3.-HELLYWOOD", QualityTypes.DVD, false },
            new object[] { "WEEDS.S03E01-06.DUAL.BDRip.AC3.-HELLYWOOD", QualityTypes.DVD, false },
            new object[] { "Two.and.a.Half.Men.S08E05.720p.HDTV.X264-DIMENSION", QualityTypes.HDTV720p, false },
            new object[] { "this has no extention or periods HDTV", QualityTypes.SDTV, false },
            new object[] { "Chuck.S04E05.HDTV.XviD-LOL", QualityTypes.SDTV, false },
            new object[] { "The.Girls.Next.Door.S03E06.DVDRip.XviD-WiDE", QualityTypes.DVD, false },
            new object[] { "The.Girls.Next.Door.S03E06.DVD.Rip.XviD-WiDE", QualityTypes.DVD, false },
            new object[] { "The.Girls.Next.Door.S03E06.HDTV-WiDE", QualityTypes.SDTV, false },
            new object[] { "Degrassi.S10E27.WS.DSR.XviD-2HD", QualityTypes.SDTV, false },
            new object[] { "Sonny.With.a.Chance.S02E15.720p.WEB-DL.DD5.1.H.264-SURFER", QualityTypes.WEBDL720p, false },
            new object[] { "Sonny.With.a.Chance.S02E15.720p", QualityTypes.HDTV720p, false },
            new object[] { "Sonny.With.a.Chance.S02E15.mkv", QualityTypes.HDTV720p, false },
            new object[] { "Sonny.With.a.Chance.S02E15.avi", QualityTypes.SDTV, false },
            new object[] { "Sonny.With.a.Chance.S02E15.xvid", QualityTypes.SDTV, false },
            new object[] { "Sonny.With.a.Chance.S02E15.divx", QualityTypes.SDTV, false },
            new object[] { "Sonny.With.a.Chance.S02E15", QualityTypes.Unknown, false },
            new object[] { "Chuck - S01E04 - So Old - Playdate - 720p TV.mkv", QualityTypes.HDTV720p, false },
            new object[] { "Chuck - S22E03 - MoneyBART - HD TV.mkv", QualityTypes.HDTV720p, false },
            new object[] { "Chuck - S01E03 - Come Fly With Me - 720p BluRay.mkv", QualityTypes.Bluray720p, false },
            new object[] { "Chuck - S01E03 - Come Fly With Me - 1080p BluRay.mkv", QualityTypes.Bluray1080p, false },
            new object[] { "Chuck - S11E06 - D-Yikes! - 720p WEB-DL.mkv", QualityTypes.WEBDL720p, false },
            new object[] { "WEEDS.S03E01-06.DUAL.BDRip.XviD.AC3.-HELLYWOOD.avi", QualityTypes.DVD, false },
            new object[] { "WEEDS.S03E01-06.DUAL.BDRip.XviD.AC3.-HELLYWOOD.avi", QualityTypes.DVD, false },
            new object[] { "Law & Order: Special Victims Unit - 11x11 - Quickie", QualityTypes.Unknown, false },
            new object[] { "(<a href=\"http://www.newzbin.com/browse/post/6076286/nzb/\">NZB</a>)", QualityTypes.Unknown, false },
            new object[] { "S07E23 - [HDTV-720p].mkv ", QualityTypes.HDTV720p, false },
            new object[] { "S07E23 - [WEBDL].mkv ", QualityTypes.WEBDL720p, false },
            new object[] { "S07E23.mkv ", QualityTypes.HDTV720p, false },
            new object[] { "S07E23 .avi ", QualityTypes.SDTV, false },
            new object[] { "WEEDS.S03E01-06.DUAL.XviD.Bluray.AC3.-HELLYWOOD.avi", QualityTypes.DVD, false },
            new object[] { "WEEDS.S03E01-06.DUAL.Bluray.AC3.-HELLYWOOD.avi", QualityTypes.Bluray720p, false },
            new object[] { "The Voice S01E11 The Finals 1080i HDTV DD5.1 MPEG2-TrollHD", QualityTypes.Unknown, false },
            new object[] { "Nikita S02E01 HDTV XviD 2HD", QualityTypes.SDTV, false },
            new object[] { "Gossip Girl S05E11 PROPER HDTV XviD 2HD", QualityTypes.SDTV, true },
            new object[] { "The Jonathan Ross Show S02E08 HDTV x264 FTP", QualityTypes.SDTV, false },
            new object[] { "White.Van.Man.2011.S02E01.WS.PDTV.x264-TLA", QualityTypes.SDTV, false },
            new object[] { "White.Van.Man.2011.S02E01.WS.PDTV.x264-REPACK-TLA", QualityTypes.SDTV, true },
            new object[] { "WEEDS.S03E01-06.DUAL.XviD.Bluray.AC3-REPACK.-HELLYWOOD.avi", QualityTypes.DVD, true },
            new object[] { "Pawn Stars S04E87 REPACK 720p HDTV x264 aAF", QualityTypes.HDTV720p, true },
            new object[] { "The Real Housewives of Vancouver S01E04 DSR x264 2HD", QualityTypes.SDTV, false },
            new object[] { "Vanguard S01E04 Mexicos Death Train DSR x264 MiNDTHEGAP", QualityTypes.SDTV, false },
            new object[] { "Vanguard S01E04 Mexicos Death Train 720p WEB DL", QualityTypes.WEBDL720p, false },
            new object[] { "Hawaii Five 0 S02E21 720p WEB DL DD5 1 H 264", QualityTypes.WEBDL720p, false },
            new object[] { "Castle S04E22 720p WEB DL DD5 1 H 264 NFHD", QualityTypes.WEBDL720p, false },
            new object[] { "Fringe S04E22 720p WEB-DL DD5.1 H264-EbP.mkv", QualityTypes.WEBDL720p, false },
            new object[] { "CSI NY S09E03 1080p WEB DL DD5 1 H264 NFHD", QualityTypes.WEBDL1080p, false },
            new object[] { "Two and a Half Men S10E03 1080p WEB DL DD5 1 H 264 NFHD", QualityTypes.WEBDL1080p, false },
            new object[] { "Criminal.Minds.S08E01.1080p.WEB-DL.DD5.1.H264-NFHD", QualityTypes.WEBDL1080p, false },
            new object[] { "Its.Always.Sunny.in.Philadelphia.S08E01.1080p.WEB-DL.proper.AAC2.0.H.264", QualityTypes.WEBDL1080p, true },
            new object[] { "Two and a Half Men S10E03 1080p WEB DL DD5 1 H 264 REPACK NFHD", QualityTypes.WEBDL1080p, true },
            new object[] { "Glee.S04E09.Swan.Song.1080p.WEB-DL.DD5.1.H.264-ECI", QualityTypes.WEBDL1080p, false },
            new object[] { "Elementary.S01E10.The.Leviathan.480p.WEB-DL.x264-mSD", QualityTypes.WEBDL480p, false },
            new object[] { "Glee.S04E10.Glee.Actually.480p.WEB-DL.x264-mSD", QualityTypes.WEBDL480p, false },
            new object[] { "The.Big.Bang.Theory.S06E11.The.Santa.Simulation.480p.WEB-DL.x264-mSD", QualityTypes.WEBDL480p, false },
            new object[] { "The.Big.Bang.Theory.S06E11.The.Santa.Simulation.1080p.WEB-DL.DD5.1.H.264", QualityTypes.WEBDL1080p, false },
            new object[] { "DEXTER.S07E01.ARE.YOU.1080P.HDTV.X264-QCF", QualityTypes.HDTV1080p, false },
            new object[] { "DEXTER.S07E01.ARE.YOU.1080P.HDTV.x264-QCF", QualityTypes.HDTV1080p, false },
            new object[] { "DEXTER.S07E01.ARE.YOU.1080P.HDTV.proper.X264-QCF", QualityTypes.HDTV1080p, true },
            new object[] { "Dexter - S01E01 - Title [HDTV]", QualityTypes.HDTV720p, false },
            new object[] { "Dexter - S01E01 - Title [HDTV-720p]", QualityTypes.HDTV720p, false },
            new object[] { "Dexter - S01E01 - Title [HDTV-1080p]", QualityTypes.HDTV1080p, false }
        };

        public static object[] SelfQualityParserCases =
        {
            new object[] { QualityTypes.SDTV },
            new object[] { QualityTypes.DVD },
            new object[] { QualityTypes.WEBDL480p },
            new object[] { QualityTypes.HDTV720p },
            new object[] { QualityTypes.WEBDL720p },
            new object[] { QualityTypes.WEBDL1080p },
            new object[] { QualityTypes.Bluray720p },
            new object[] { QualityTypes.Bluray1080p }
        };

        public static object[] AnimeQualityParserCases =
        {
            new object[] { "[Zorori-Project] Tetsunoshin - 26 [H264][46474F0A].avi", QualityTypes.SDTV, false },
            new object[] { "[Zorori-Project] Gon - 16 [H264][1440x810][C27814A8].avi", QualityTypes.HDTV720p, false },
            new object[] { "[yibis] One Piece 576 [480p][1BD9CF81].mp4", QualityTypes.SDTV, false },
            new object[] { "[Commie] Ano Natsu de Matteru - NCOP [BD 720p AAC] [8110CADE].mkv", QualityTypes.Bluray720p, false },
            new object[] { "[HorribleSubs] Naruto SD - Rock Lee no Seishun Full-Power Ninden - 39 [1080p].mkv", QualityTypes.HDTV1080p, false },
            new object[] { "[Mezashite] Aikatsu! - 12 [720p][7842F590].mkv", QualityTypes.HDTV720p, false },
            new object[] { "[Commie] Muv-Luv Alternative - Total Eclipse - 24 [2E4E4C2C].mkv", QualityTypes.HDTV720p, false },
            new object[] { "[Meguca] Puella Magi Madoka Magica - 06 [BD][h264-720p AAC][7D4146A9].mkv", QualityTypes.Bluray720p, false },
            new object[] { "[FFF] Dog Days S2 - 02 [BD][720p-AAC][D77BD419].mkv", QualityTypes.Bluray720p, false },
            new object[] { "[DmonHiro] Jormungand 11 - [BD.720p][B872C2BC].mkv", QualityTypes.Bluray720p, false },
            new object[] { "[Hatsuyuki] History's Strongest Disciple Kenichi OVA - 03 [DVD 848x480][D17A0C66].avi", QualityTypes.DVD, false },
            new object[] { "[EveTaku] Sword Art Online - 22 (1280x720 x264-Hi10P AAC)[DDA6C43C].mkv", QualityTypes.HDTV720p, false },
            new object[] { "[Hatsuyuki] Robotics Notes - 10 [848x480][71845F54].avi", QualityTypes.SDTV, false },
            new object[] { "[AFFTW-Hatsuyuki] To Love-Ru Darkness - 10v2 [848x480][CF728264].avi", QualityTypes.SDTV, true },
        };

        [Test, TestCaseSource("QualityParserCases")]
        public void quality_parse(string postTitle, QualityTypes quality, bool proper)
        {
            var result = Parser.ParseQuality(postTitle);
            result.Quality.Should().Be(quality);
            result.Proper.Should().Be(proper);
        }

        [Test, TestCaseSource("SelfQualityParserCases")]
        public void parsing_our_own_quality_enum(QualityTypes quality)
        {
            var fileName = String.Format("My series S01E01 [{0}]", quality);
            var result = Parser.ParseQuality(fileName);
            result.Quality.Should().Be(quality);
        }

        [Test, TestCaseSource("AnimeQualityParserCases")]
        public void anime_quality_parse(string postTitle, QualityTypes quality, bool proper)
        {
            var result = Parser.ParseQuality(postTitle);
            result.Quality.Should().Be(quality);
            result.Proper.Should().Be(proper);
        }
    }
}