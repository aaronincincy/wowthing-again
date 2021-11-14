import type { StaticDataSetCategory } from '.'

export interface CollectionContext {
    route: string
    thingType: string
    thingMap: Record<number, number>
    userHas: Record<number, boolean>
    sets: StaticDataSetCategory[][]
}
