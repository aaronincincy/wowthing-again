import App from './apps/home/App.svelte'

const target = document.querySelector('#app');
const app = target ? new App({ target }) : null

export default app
