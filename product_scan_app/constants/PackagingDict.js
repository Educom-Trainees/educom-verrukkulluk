const PackagingDict = {
    0: 'Los',
    1: 'Pak',
    2: 'Duo-pack',
    3: 'Doos',
    4: '4-pack',
    5: 'Flex',
    6: '6-pack',
    7: 'Blik',
    8: 'Kuipje',
    9: 'Zak',
    10: 'Net',
    11: 'Pot',
};

export function GetPackagingValue(number) {
    return PackagingDict[number] || 'Onbekend';
}