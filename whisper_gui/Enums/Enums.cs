﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace whisper_gui.Enums
{
    public enum Status
    {
        Pending,
        Processing,
        Completed,
        Failed
    }

    public enum WhisperOutputFormats
    {
        txt,
        vtt,
        srt,
        tsv,
        json,
        all
    }
    public enum WhisperDevices
    {
        cpu,
        cuda
    }

    public enum WhisperModels
    {
        tiny,
        @base,
        small,
        medium,
        large
    }

    public enum WhisperLanguages
    {
        Afrikaans,
        Albanian,
        Amharic,
        Arabic,
        Armenian,
        Assamese,
        Azerbaijani,
        Bashkir,
        Basque,
        Belarusian,
        Bengali,
        Bosnian,
        Breton,
        Bulgarian,
        Burmese,
        Cantonese,
        Castilian,
        Catalan,
        Chinese,
        Croatian,
        Czech,
        Danish,
        Dutch,
        English,
        Estonian,
        Faroese,
        Finnish,
        Flemish,
        French,
        Galician,
        Georgian,
        German,
        Greek,
        Gujarati,
        Haitian,
        HaitianCreole,
        Hausa,
        Hawaiian,
        Hebrew,
        Hindi,
        Hungarian,
        Icelandic,
        Indonesian,
        Italian,
        Japanese,
        Javanese,
        Kannada,
        Kazakh,
        Khmer,
        Korean,
        Lao,
        Latin,
        Latvian,
        Letzeburgesch,
        Lingala,
        Lithuanian,
        Luxembourgish,
        Macedonian,
        Malagasy,
        Malay,
        Malayalam,
        Maltese,
        Mandarin,
        Maori,
        Marathi,
        Moldavian,
        Moldovan,
        Mongolian,
        Myanmar,
        Nepali,
        Norwegian,
        Nynorsk,
        Occitan,
        Panjabi,
        Pashto,
        Persian,
        Polish,
        Portuguese,
        Punjabi,
        Pushto,
        Romanian,
        Russian,
        Sanskrit,
        Serbian,
        Shona,
        Sindhi,
        Sinhala,
        Sinhalese,
        Slovak,
        Slovenian,
        Somali,
        Spanish,
        Sundanese,
        Swahili,
        Swedish,
        Tagalog,
        Tajik,
        Tamil,
        Tatar,
        Telugu,
        Thai,
        Tibetan,
        Turkish,
        Turkmen,
        Ukrainian,
        Urdu,
        Uzbek,
        Valencian,
        Vietnamese,
        Welsh,
        Yiddish,
        Yoruba
    }
}
