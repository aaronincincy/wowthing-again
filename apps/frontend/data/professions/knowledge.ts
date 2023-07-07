import { Profession } from '@/enums'

type DragonflightKnowledge = {
    name: string
    shortName: string
    icon: string
    masters: Profession[]
    reputationId?: number
}

export const dragonflightKnowledge: DragonflightKnowledge[] = [
    {
        name: "Artisan's Consortium Books",
        shortName: 'AC',
        icon: 'spell/339979', // Booksmart
        masters: [],
    },
    {
        name: 'Dragonscale Expedition Books',
        shortName: 'DE',
        icon: 'achievement/16522', // A True Explorer
        masters: [],
        reputationId: 2507,
    },
    {
        name: 'Iskaaran Tuskar Books',
        shortName: 'IT',
        icon: 'achievement/16529', // Joining the Community
        masters: [],
        reputationId: 2511,
    },
    {
        name: 'Maruuk Centaur Books',
        shortName: 'MC',
        icon: 'achievement/16528', // Joining the Khansguard
        masters: [],
        reputationId: 2503,
    },
    {
        name: 'Valdrakken Accord Books',
        shortName: 'VA',
        icon: 'achievement/16530', // Ally of the Flights
        masters: [],
        reputationId: 2510,
    },
    null,
    {
        name: 'Zaralek Cavern Books',
        shortName: 'ZC',
        icon: 'spell/339979', // Booksmart
        masters: [],
    },
    null,
    {
        name: 'Valdrakken',
        shortName: 'VD',
        icon: 'achievement/16530',
        masters: [
            Profession.Tailoring,
        ],
    },
    {
        name: 'Azure Span',
        shortName: 'AS',
        icon: 'achievement/16336',
        masters: [
            Profession.Engineering,
            Profession.Inscription,
            Profession.Jewelcrafting,
        ],
    },
    {
        name: "Ohn'ahran Plains",
        shortName: 'OP',
        icon: 'achievement/15394',
        masters: [
            Profession.Enchanting,
            Profession.Herbalism,
            Profession.Leatherworking,
        ],
    },
    {
        name: 'Thaldraszus',
        shortName: 'TD',
        icon: 'achievement/16363',
        masters: [
            Profession.Mining,
        ],
    },
    {
        name: 'The Waking Shores',
        shortName: 'WS',
        icon: 'achievement/16334',
        masters: [
            Profession.Alchemy,
            Profession.Blacksmithing,
            Profession.Skinning,
        ],
    },
    null,
    {
        name: 'Zaralek Cavern',
        shortName: 'ZC',
        icon: 'achievement/16401',
        masters: [],
    },
]
