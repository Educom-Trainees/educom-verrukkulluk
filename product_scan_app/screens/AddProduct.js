import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, TouchableOpacity } from 'react-native';
import { ScrollView, TextInput } from 'react-native-gesture-handler';
import { useState, useRef } from 'react';
import AllergenDropdownMenu from '../components/AllergenDropdownMenu';

const AddProductScreen = () => {
  const quantityRef = useRef(0);
  const minQuantityRef = useRef(0);
  const [quantity, setQuantity] = useState(0);
  const [minQuantity, setMinQuantity] = useState(0);
  const [selectedAllergens, setSelectedAllergens] = useState([]);

  const handleSave = () => {
    console.log('Product bewaard!');
  };

  const handleQuantityIncrement = () => {
    setQuantity(quantity + 1);
  };

  const handleQuantityDecrement = () => {
    if (quantity > 0) {
      setQuantity(quantity - 1);
    };
  };

  const handleMinQuantityIncrement = () => {
    setMinQuantity(minQuantity + 1);
  };

  const handleMinQuantityDecrement = () => {
    if (minQuantity > 0) {
      setMinQuantity(minQuantity - 1);
    };
  };

  const handleAllergenSelect = (selectedOptions) => {
    setSelectedAllergens(selectedOptions);
  };

  return (
    <SafeAreaView style={styles.container}>
      <ScrollView contentContainerStyle={styles.scrollview} showVerticalScrollIndicator={false}>
        <Text style={[styles.heading, styles.extramargintop]}>Titel</Text>
        <TextInput style={styles.input} />

        <Text style={styles.heading}>Beschrijving</Text>
        <TextInput style={[styles.input, styles.multilinetext]} multiline/>

        <Text style={styles.heading}>Type</Text>
        <TextInput style={styles.input} />

        <Text style={styles.heading}>Kwantiteit</Text>
        <View style={styles.quantitycontainer}>
          <TouchableOpacity onPress={handleQuantityDecrement}>
            <Text style={[styles.quantitybutton, styles.redquantitybutton]}>-</Text>
          </TouchableOpacity>
          <TextInput 
            style={[styles.input, styles.quantityinput]}
            value={quantity.toString()}
            onChangeText={(text) => {
              const tempValue = text.replace(/[^0-9.]/g, '');
              if (tempValue === '') {
                setQuantity(0);
              } else {
                setQuantity(tempValue);
              }
            }}
            keyboardType='decimal-pad'
          />
          <TouchableOpacity onPress={handleQuantityIncrement}>
            <Text style={[styles.quantitybutton, styles.greenquantitybutton]}>+</Text>
          </TouchableOpacity>
        </View>

        <Text style={styles.heading}>Minimum kwantiteit</Text>
        <View style={styles.quantitycontainer}>
          <TouchableOpacity onPress={handleMinQuantityDecrement}>
            <Text style={[styles.quantitybutton, styles.redquantitybutton]}>-</Text>
          </TouchableOpacity>
          <TextInput 
            style={[styles.input, styles.quantityinput]}
            value={minQuantity.toString()}
            onChangeText={(text) => {
              const tempValue = text.replace(/[^0-9.]/g, '');
              if (!isNaN(tempValue)) {
                setMinQuantity(tempValue);
              } else {
                setMinQuantity(0);
              }
            }}
            keyboardType='decimal-pad'
          />
          <TouchableOpacity onPress={handleMinQuantityIncrement}>
            <Text style={[styles.quantitybutton, styles.greenquantitybutton]}>+</Text>
          </TouchableOpacity>
        </View>

        <Text style={styles.heading}>Verpakking</Text>
        <TextInput style={styles.input} />

        <Text style={styles.heading}>Prijs</Text>
        <TextInput
          style={styles.input}
          keyboardType='numeric'
        />

        <AllergenDropdownMenu onSelect={handleAllergenSelect} />

        <TouchableOpacity style={styles.savebutton} onPress={handleSave}>
          <Text style={styles.savebuttontext}>Opslaan</Text>
        </TouchableOpacity>
      </ScrollView>
    </SafeAreaView>
  );
};

export default AddProductScreen;

const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: '#fff0d6',
      paddingTop: StatusBar.currentHeight,
    },
    scrollview: {
      backgroundColor: '#fff0d6',
      paddingTop: 15,
      paddingBottom: 60,
    },
    extramargintop: {
      marginTop: 15,
    },
    heading: {
      alignSelf: 'center',
      fontSize: 16,
      fontWeight: 'bold',
    },
    input: {
      height: 40,
      margin: 12,
      padding: 10,
      borderWidth: 1,
      backgroundColor: '#ffffff',
    },
    multilinetext: {
      minHeight: 80,
      textAlignVertical: "top",
    },
    quantitycontainer: {
      flexDirection: 'row',
      alignItems: 'center',
      justifyContent: 'center',
      paddingHorizontal: 10,
    },
    quantitybutton: {
      backgroundColor: '#ffffff',
      fontSize: 20,
      fontWeight: 'bold',
      width: 30,
      height: 30,
      textAlign: 'center',
      textAlignVertical: 'center',
      borderRadius: 15,
    },
    quantityinput: {
      textAlign: 'center',
      textAlignVertical: 'center',
      borderRadius: 10,
    },
    redquantitybutton: {
      backgroundColor: '#dc3439',
    },
    greenquantitybutton: {
      backgroundColor: '#95b82d',
    },
    savebutton: {
      alignSelf: 'center',
      backgroundColor: '#95b82d',
      padding: 10,
      borderRadius: 5,
      marginTop: 25,
      width: '80%',
    },
    savebuttontext: {
      color: 'white',
      textAlign: 'center',
      fontSize: 16,
      fontWeight: 'bold',
    },
});