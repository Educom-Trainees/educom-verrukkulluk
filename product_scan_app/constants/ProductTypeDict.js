const ProductTypeDict = {
    1: 'ML',
    2: 'Gram',
    3: 'Stuks',
}

export function GetProductType(number) {
    return ProductTypeDict[number] || 'Onbekende kwantiteitstype';
}