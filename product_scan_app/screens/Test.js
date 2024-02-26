import React, { useState } from 'react';
import { TextInput, StyleSheet } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';

const TestScreen = () => {
    const [searchText, setSearchText] = useState('');
    
    const updateSearch = (search) => {
        setSearchText(search);
    };

    return (
        <SafeAreaView style={styles.container}>
        <TextInput
            style={styles.searchbar}
            placeholder='Zoeken'
            onChangeText={updateSearch}
            clearButtonMode='always'
        />
    </SafeAreaView>
    );
};

export default TestScreen;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
    },
    searchbar: {
        fontSize: 20,
        textAlign: 'center',
        height: 50,
        width: '95%',
        backgroundColor: '#ffffff',
        textDecorationLine: 'none',
        marginBottom: 10,
    },
});