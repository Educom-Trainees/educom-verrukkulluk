import React from 'react';
import { useState } from "react";
import { StyleSheet, View, Text, Modal, TouchableOpacity, Image } from 'react-native';
import { ScrollView } from 'react-native-gesture-handler';


const AllergenDropdownMenu = ({ onSelect }) => {
    
    const [modalVisible, setModalVisible] = useState(false);
    const [selectedOptions, setSelectedOptions] = useState([]);

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
        const isFirstOption = option. value === options[0].value;
        const isSelected = selectedOptions.some((selectedOption) => selectedOption.value === option.value);

        if (isSelected) {
            const updatedOptions = selectedOptions.filter((selectedOption) => selectedOption.value !== option.value);
            setSelectedOptions(updatedOptions);
        } else {
            let updatedOptions;

            //If 'geen' is selected, all other options are deselected. If anything else is selected, 'geen' is deselected.
            if (isFirstOption) {
                updatedOptions = [option];
            } else {
                updatedOptions = selectedOptions.filter((selectedOption) => selectedOption.value !== options[0].value);
                updatedOptions.push(option);
            }
            setSelectedOptions(updatedOptions);
        }
    };

    const handleSave = () => {
        setModalVisible(false);
        if (selectedOptions.length > 0) {
            onSelect(selectedOptions);
        }
    };

    return (
        <View style={styles.container}>
            <Text style={styles.heading}>Aanwezige allergenen</Text>
            <ScrollView
                horizontal={true}
                style={styles.selectedoptionsrow}
                showsHorizontalScrollIndicator={false}>
                    {selectedOptions.length > 0 && selectedOptions.map((option) => (
                        <Image key={option.value} source={option.image} style={styles.optionimagesmall} />
                    ))}
            </ScrollView>
            <TouchableOpacity onPress={() => setModalVisible(true)} style={styles.alterbutton}>
                <Text style={[styles.buttontext]}>Voeg toe/Wijzigen</Text>
            </TouchableOpacity>
        
            <Modal transparent={true} visible={modalVisible} animationType="slide">
                <View style={styles.modalcontainer}>
                    <ScrollView contentContainerStyle={styles.scrollview} style={styles.scrollviewwide}>
                        {options.map((option) => (
                            <TouchableOpacity
                            key={option.value}
                            onPress={() => handleOptionPress(option)}
                            style={[
                                styles.modaloption, 
                                selectedOptions.some((selectedOption) => selectedOption.value === option.value) && styles.selectedoption]}
                            >
                                <Image source={option.image} style={styles.optionimage} />
                            </TouchableOpacity>
                        ))}
                    </ScrollView>
                    <TouchableOpacity onPress={handleSave} style={styles.savebutton}>
                        <Text style={styles.buttontext}>Opslaan</Text>
                    </TouchableOpacity>
                </View>
            </Modal>
        </View>
    );
};

export default AllergenDropdownMenu;

const styles = StyleSheet.create({
    addallergenbutton: {
        backgroundColor: '#ffffff',
        fontSize: 20,
        fontWeight: 'bold',
        width: 30,
        height: 30,
        textAlign: 'center',
        textAlignVertical: 'center',
        borderRadius: 15,
        backgroundColor: '#95b82d',
    },
    alterbutton: {
        alignSelf: 'center',
        backgroundColor: '#95b82d',
        padding: 10,
        borderRadius: 5,
        width: '40%',
    },
    buttontext: {
        color: 'white',
        textAlign: 'center',
        fontSize: 16,
        fontWeight: 'bold',
    },
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
    heading: {
        alignSelf: 'center',
        fontSize: 16,
        fontWeight: 'bold',
        marginBottom: -10,
    },
    modalcontainer: {
        flex: 1,
        justifyContent: 'center',
        alignItems: 'center',
        backgroundColor: '#fff0d6',
    },
    modaloption: {
        flexDirection: 'row',
        alignItems: 'center',
        padding: 10,
        borderBottomWidth: 1,
        borderColor: '#fff0d6',
        opacity: 0.2,
    },
    optionimage: {
        width: 100,
        height: 100,
    },
    optionimagesmall: {
        width: 75,
        height: 75,
    },
    savebutton: {
        alignSelf: 'center',
        backgroundColor: '#95b82d',
        padding: 10,
        borderRadius: 5,
        marginVertical: 20,
        width: '80%',
    },
    scrollview: {
        alignItems: 'center',
        justifyContent: 'center',
    },
    scrollviewwide: {
        width: '100%',
    },
    selectedoption: {
        opacity: 1,
    },
    selectedoptionsrow: {
        flexDirection: 'row',
        marginVertical: 10,
    },
});