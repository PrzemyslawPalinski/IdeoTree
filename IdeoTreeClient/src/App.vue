<template>
    <div v-if="!loading">
        <button class="expandButton" @click="expandNodes">Expand / Close All</button>
        <TreeBrowser :node="root"
                     :isOpen="isOpen"
                     @update="updateRoot"/>


        Node to move: <input v-model="idFrom"/>
        New parent node: <input  v-model="idTo"/>
        <button class="expandButton"  @click="moveNode">Move node</button>
    </div>
    <p v-if="loading">
        Still loading..
    </p>
    <p v-if="error">
        {{error.json}}
    </p>
</template>

<script>
    import { ref, onMounted } from 'vue';
    import TreeBrowser from './components/TreeBrowser.vue'

    export default {
        name: 'App',
        data() {
            return {
                isOpen: false,
                idFrom: 0,
                idTo: 0
            }
        },
        methods: {
            updateRoot(data) {
                this.root = data;
            },
            expandNodes() {
                this.isOpen = !this.isOpen;
            },
            moveNode() {
                fetch(`http://localhost:5154/api/treenode/${this.idFrom}/${this.idTo}`, { method: 'PUT' })
                    .then(res => res.json())
                    .then(data => this.root=data);
            }
        },
        components: {
            TreeBrowser
        },
        setup() {
            const root = ref(null);
            const loading = ref(true);
            const error = ref(null);
            function fetchData() {
                loading.value = true;
                return fetch('http://localhost:5154/api/treenode', {
                    method: 'get',
                }).then(res => {
                    if (!res.ok) {
                        const error = new Error(res.statusText);
                        error.json = res.json();
                        throw error;
                    }
                    return res.json();
                }).then(json => {
                    root.value = json;
                }).catch(err => {
                    error.value = err;
                    if (err.json) {
                        return err.json.then(json => {
                            error.value.message = json.message;
                        });
                    }
                }).then(() => {
                    loading.value = false;
                });
            }

            onMounted(() => {
                fetchData();
            })
            return {
                root,
                loading,
                error
            };
        }
    }
</script>

<style>
    #app {
        font-family: Avenir, Helvetica, Arial, sans-serif;
        -webkit-font-smoothing: antialiased;
        -moz-osx-font-smoothing: grayscale;
        text-align: center;
        color: white;
        margin-top: 60px;
    }

    body {
        background-color: #121212;
    }
    .expandButton {
        background-color: #212121;
        color: white;
        border-color: white;
        padding: 5px;
        border-radius: 25px;
        margin: 10px;
    }
</style>
