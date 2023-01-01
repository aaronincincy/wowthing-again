import { replace } from 'svelte-spa-router'


export default function getSavedRoute(
    route: string,
    slug1?: string,
    slug2?: string,
    sidebarId = 'sub-sidebar',
    lastChild = false
): void {
    const key = `route-${route}`
    if (slug1 === null) {
        const saved = localStorage.getItem(key)
        const subSidebar = document.getElementById(sidebarId)
        if (subSidebar !== null) {
            if (!!saved && saved !== 'undefined') {
                replace(`/${route}/${saved}`)
                return
            }

            const link = subSidebar.querySelector(lastChild ? 'a:last-child' : 'a')
            replace(link.getAttribute('href').replace('#', ''))
        }
        else {
            console.log("couldn't find sidebar??", sidebarId)
        }
    }
    else {
        localStorage.setItem(key, slug2 ? `${slug1}/${slug2}` : slug1)
    }
}
