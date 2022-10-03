import { WritableFancyStore } from '@/types'
import { ItemDataItem, type ItemData } from '@/types/data/item'


export class ItemDataStore extends WritableFancyStore<ItemData> {
    get dataUrl(): string {
        return document.getElementById('app').getAttribute('data-item')
    }

    initialize(data: ItemData) {
        // console.time('ItemDataStore.initialize')

        const appearanceIds: Record<number, Record<number, boolean>> = {}

        data.items = {}
        for (const itemArray of data.rawItems) {
            const obj = new ItemDataItem(...itemArray)
            data.items[obj.id] = obj

            for (const appearance of Object.values(obj.appearances)) {
                appearanceIds[appearance.appearanceId] ||= {}
                appearanceIds[appearance.appearanceId][obj.id] = true
            }
        }
        data.rawItems = null

        data.appearanceToItems = Object.fromEntries(
            Object.entries(appearanceIds)
                .map(([id, items]) => [
                    parseInt(id),
                    Object.keys(items)
                        .map((itemId) => parseInt(itemId))
                ])
        )

        // console.timeEnd('ItemDataStore.initialize')
    }
}

export const itemStore = new ItemDataStore()
