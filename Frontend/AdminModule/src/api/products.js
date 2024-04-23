import {fetchData} from './fetch'

// function to GET product info from DB
export function getProducts () {
    return fetchData('../api/products');
}

// function to POST a new product to DB
export function postProduct (productJSON) {
    return fetch('../api/products', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: productJSON,
    });
}

// function to update (PUT) an existing product
export function putProduct ({id, productJSON}) { // destructoring necessary as react-query mutate functions only take one input
    return fetch('../api/products/'+id, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: productJSON,
    });
}

// function to DELETE a product
export function deleteProduct(id) {
    return fetch('../api/products/' + id, {
        method: 'DELETE',
        headers: { 'Content-Type': 'application/json' },
    })
}

// function to toggle a product's active flag
export function toggleActiveProduct(id) {
    return fetch('../api/products/' + id + '/active', {
        method: 'PATCH',
        headers: { 'Content-Type': 'application/json' },
    })
}

// function to GET ingredient type info from DB
export function getIngredientTypes () {
    return fetchData('../api/products/ingredienttypes');
}

// function to POST a new kitchen type to DB
export function postIngredientType (ingredientJSON) {
    return fetch('../api/products/ingredienttypes', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: ingredientJSON,
    });
}

// function to update (PUT) an existing kitchen type
export function putIngredientType ({id, ingredientJSON}) { // destructoring necessary as react-query mutate functions only take one input
    return fetch('../api/products/ingredienttypes/'+id, {
        method: 'PUT',
        headers: { 'Content-Type': 'application/json' },
        body: ingredientJSON,
    });
}