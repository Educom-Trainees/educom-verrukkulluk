import React from 'react';
import { useState } from "react";
import { StyleSheet, View, Text, Modal, TouchableOpacity, Image } from 'react-native';


const AllergenDropdownMenu = () => {
    
    const [modalVisible, setModalVisible] = useState(false);
    const [selectedOption, setSelectedOption] = useState(null);

    const options = [
        { label: 'geen', value: 'geen', image: require('../assets/images/allergens/geen.png') },
        { label: 'ei', value: 'ei', image: require('../assets/images/allergens/ei.png') },
        { label: 'gluten', value: 'gluten', image: require('../assets/images/allergens/gluten.png') },
        { label: 'lupine', value: 'lupine', image: require('../assets/images/allergens/lupine.png') },
        { label: 'melk', value: 'melk', image: require('../assets/images/allergens/melk.png') },
        { label: 'mosterd', value: 'mosterd', image: require('../assets/images/allergens/mosterd.png') },
        { label: 'noten', value: 'noten', image: require('../assets/images/allergens/noten.png') },
        { label: 'pindas', value: 'pindas', image: require('../assets/images/allergens/pindas.png') },
        { label: 'schaaldieren', value: 'schaaldieren', image: require('../assets/images/allergens/schaaldieren.png') },
        { label: 'selderij', value: 'selderij', image: require('../assets/images/allergens/selderij.png') },
        { label: 'sesamzaad', value: 'sesamzaad', image: require('../assets/images/allergens/sesamzaad.png') },
        { label: 'soja', value: 'soja', image: require('../assets/images/allergens/soja.png') },
        { label: 'vis', value: 'vis', image: require('../assets/images/allergens/vis.png') },
        { label: 'vlees', value: 'vlees', image: require('../assets/images/allergens/vlees.png') },
        { label: 'weekdieren', value: 'weekdieren', image: require('../assets/images/allergens/weekdieren.png') },
        { label: 'zwavel', value: 'zwavel', image: require('../assets/images/allergens/zwavel.png') },
    ];

    const handleOptionPress = (option) => {
        setSelectedOption(option);
        setModalVisible(false);
    };


    return (
        <View style={styles.container}>
            <TouchableOpacity onPress={() => setModalVisible(true)} style={styles.dropdownButton}>
                {selectedOption ? (
                <Image source={selectedOption.image} style={styles.optionImage} />
                ) : (
                <Text>Selecteer een allergeen</Text>
                )}
            </TouchableOpacity>
        
            <Modal transparent={true} visible={modalVisible} animationType="slide">
                <View style={styles.modalContainer}>
                    {options.map((option) => (
                        <TouchableOpacity
                        key={option.value}
                        onPress={() => handleOptionPress(option)}
                        style={styles.modalOption}
                        >
                        <Image source={option.image} style={styles.optionImage} />
                        </TouchableOpacity>
                    ))}
                </View>
            </Modal>
        </View>
    );
};

export default AllergenDropdownMenu;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center',
    },
    dropdownbutton: {
        padding: 10,
        borderWidth: 1,
        borderRadius: 5,
    },
    modalContainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#fff0d6',
    },
    modalOption: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: 10,
        borderBottomWidth: 1,
        borderColor: '#ddd',
    },
    optionImage: {
        width: 100,
        height: 100,
    },
});