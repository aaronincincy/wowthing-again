import { Profession } from '@/enums'
import type { DragonflightProfession } from '@/types/data'


export const dragonflightTailoring: DragonflightProfession = {
    id: Profession.Tailoring,
    hasCraft: true,
    hasOrders: true,
    masterQuestId: 70260,
    bookQuests: [
        {
            itemId: 200975, // Artisan's Consortium, Preferred
            questId: 71903,
        },
        {
            itemId: 201271, // Artisan's Consortium, Valued
            questId: 71914,
        },
        {
            itemId: 201282, // Artisan's Consortium, Esteemed
            questId: 71925,
        },
    ],
    dropQuests: [
        {
            itemId: 193898, // Umbral Bone Needle
            questId: 66386,
            source: 'Treasures',
        },
        {
            itemId: 193899, // Primalweave Spindle
            questId: 66387,
            source: 'Treasures',
        },
        {
            itemId: 198977, // Ohn'ahran Weave
            questId: 70524,
            source: 'Mobs: Centaur',
        },
        {
            itemId: 198978, // Stupidly Effective Stitchery
            questId: 70525,
            source: 'Mobs: Gnoll',
        },
        {
            itemId: 204225, // Perfect Windfeather
            questId: 74321,
            source: 'FR: Gareed',
        },
    ],
    treasureQuests: [
        {
            itemId: 206019, // Abandoned Reserve Chute
            questId: 76102,
            source: 'ZC',
        },
        {
            itemId: 201019, // Ancient Dragonweave Bolt
            questId: 70372,
            source: 'TD',
        },
        {
            itemId: 198692, // Noteworthy Scrap of Carpet
            questId: 70295,
            source: 'OP',
        },
        {
            itemId: 198680, // Decaying Brackenhide Blanket
            questId: 70284,
            source: 'AS',
        },
        {
            itemId: 206030, // Exquisitely Embroidered Banner
            questId: 76116,
            source: 'ZC',
        },
        {
            itemId: 198662, // Intriguing Bolt of Blue Cloth
            questId: 70267,
            source: 'AS',
        },
        {
            itemId: 198702, // Itinerant Singed Fabric
            questId: 70304,
            source: 'WS',
        },
        {
            itemId: 198684, // Miniature Bronze Dragonflight Banner
            questId: 70288,
            source: 'TD',
        },
        {
            itemId: 198699, // Mysterious Banner
            questId: 70302,
            source: 'WS',
        },
        {
            itemId: 201020, // Silky Surprise
            questId: 70303,
            source: 'OP',
        },
        {
            itemId: 206025, // Used Medical Wrap Kit
            questId: 76110,
            source: 'ZC',
        },
    ]
}
