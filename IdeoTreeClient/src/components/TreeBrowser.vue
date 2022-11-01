<template>
    <div class="treeView">
        <div :style="{'margin-left': `${depth * 30}px`}"
             class="node">
            <span v-if="hasChildren"
                  class="type">{{expanded ? '&#9660;' : '&#9658;'}}</span>
            <span v-else>&#9671;</span>
            <div @click="nodeExpand" v-if="!updateActive"> {{node.data}}, ID = {{node.id}} </div>
            <input v-if="updateActive"
                   v-model="data">
            <input v-if="addActive"
                   v-model="data">
            <button v-if="!updateActive && !addActive"
                    class="deleteButton"
                    @click="deleteNode">
                Delete
            </button>
            <button v-if="!updateActive && !addActive"
                    class="updateButton"
                    @click="changeToUpdate">
                Update
            </button>
            <button v-if="updateActive"
                    class="saveButton"
                    @click="updateNode">
                Save
            </button>
            <button v-if="updateActive"
                    class="cancelButton"
                    @click="changeToUpdate">
                Cancel
            </button>
            <button v-if="!addActive && !updateActive"
                    class="addButton"
                    @click="changeToAdd">
                Add child
            </button>
            <button v-if="addActive"
                    class="saveButton"
                    @click="addNode">
                Save
            </button>
            <button v-if="addActive"
                    class="cancelButton"
                    @click="changeToAdd">
                Cancel
            </button>


        </div>

        <div v-if="expanded || isOpen">
            <TreeBrowser v-for="child in node.children"
                         :key="child.data"
                         :node="child"
                         :isOpen="isOpen"
                         :moveIsActive="moveIsActive"
                         :depth="depth + 1"                       
                         @update="(data) => $emit('update', data)"
                         @nodeToMove="(node, moveIsActive) =>  $emit('nodeToMove', node, moveIsActive)"/>
            </div>
        </div>
</template>

<script>
    export default {
        name: 'TreeBrowser',
        props: {
            moveIsActive: Boolean,
            isOpen: Boolean,
            node: Object,
            depth: {
                type: Number,
                default: 0
            }
        },
        data() {
            return {
                expanded: this.isOpen,
                updateActive: false,
                addActive: false,
                data: this.node.data
            }
        },
        methods: {
            nodeExpand() {
                this.expanded = !this.expanded;
            },
            deleteNode() {
                fetch(`http://localhost:5154/api/treenode/${this.node.id}`, { method: 'DELETE' })
                    .then(res => res.json())
                    .then(data => this.$emit('update', data));
                
            },
            updateNode() {
                fetch(`http://localhost:5154/api/treenode/${this.node.id}?data=${this.data}`, { method: 'PUT' })
                    .then(res => res.json())
                    .then(data => this.$emit('update', data));
                this.updateActive = !this.updateActive;
            },
            addNode() {
                fetch(`http://localhost:5154/api/treenode/add?data=${this.data}&id=${this.node.id}`, { method: 'POST' })
                    .then(res => res.json())
                    .then(data => this.$emit('update', data));
                this.addActive = !this.addActive;
                if (!this.expanded) {
                    this.expanded = !this.expanded;
                }
            },
            changeToUpdate() {
                this.data = this.node.data;
                this.updateActive = !this.updateActive;
            },
            changeToAdd() {
                this.data = '';
                this.addActive = !this.addActive;
            },
            moveNode() {            
                this.isMoveActive = !this.isMoveActive;
                this.$emit('nodeToMove', this.node, this.moveIsActive);
            }
        },
        computed: {
            hasChildren() {
                return this.node.children.length !== 0;
            }
        },
        watch: {
            isOpen() {
                this.expanded = this.isOpen;
            }
        }
    }
</script>

<style scoped>
    .treeView {
        text-align: left;
        margin: 10px;
        border: dotted;
        padding: 10px;
    }
    .node {
        color: white;
        font-size: 18px;
        display: inline-flex;
    }
    .deleteButton {
        margin-left: 20px;
        background-color: red;
        color: white;
        border-color: orangered;
        border-radius: 12px;
    }
    span {
        margin-right: 5px;
    }
    .updateButton {
        margin-left: 20px;
        background-color: blue;
        color: white;
        border-color: blueviolet;
        border-radius: 12px;
    }
    .addButton {
        margin-left: 20px;
        background-color: green;
        color: white;
        border-color: forestgreen;
        border-radius: 12px;
    }
</style>