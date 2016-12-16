class ToDoClient{
    constructor() {
        this.baseUri = "api/todo";
    }
    getAll() {
        return fetch(this.baseUri).then((response)=> {
            const contentType = response.headers.get("content-type");
            if (contentType && contentType.indexOf("application/json") !== -1) {
                return response.json();
            }
        });
    }
    find(key) {
        return fetch(`${this.baseUri}/${key}`).then((response)=> {
            const contentType = response.headers.get("content-type");
            if (contentType && contentType.indexOf("application/json") !== -1) {
                return response.json();
            }
        });
    }
    add(item) {
        return fetch(this.baseUri, {
            method: 'post',
            body: JSON.stringify(item),
            headers: new Headers({
                'Content-Type': 'application/json'
            })
        });
    }
    update(key, item) {
        return fetch(`${this.baseUri}/${key}`, {
            method: 'put',
            body: JSON.stringify(item),
            headers: new Headers({
                'Content-Type': 'application/json'
            })
        });
    }
    remove(key) {
        return fetch(`${this.baseUri}/${key}`, {
            method: 'delete'
        });
    }
}


const client = new ToDoClient();

client.add({ key: "", name: "Do Something NOW!", isComplete: false })
    .then(()=>client.getAll())
    .then(items => {
        console.log(items);
        return client.update(items[0].key, { key: items[0].key, name: items[0].name, isComplete: true });
    }).then(()=>client.getAll())
    .then(items=> {
        console.log(items);
        return client.remove(items[0].key);
    }).then(()=>client.getAll())
    .then(console.log);