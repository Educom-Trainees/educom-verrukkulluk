import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, TouchableOpacity } from 'react-native';
import { ScrollView, TextInput } from 'react-native-gesture-handler';
import { useState, useRef } from 'react';
import AllergenDropdownMenu from '../components/AllergenDropdownMenu';
import { Picker } from '@react-native-picker/picker';
import { API_BASE_URL } from '@env';

const AddProductScreen = () => {
  const [name, setName] = useState();
  const [description, setDescription] = useState();
  const [quantity, setQuantity] = useState('');
  const [minQuantity, setMinQuantity] = useState('');
  const [selectedType, setSelectedType] = useState(1);
  const [selectedPackaging, setSelectedPackaging] = useState(0);
  const [price, setPrice] = useState();
  const [selectedAllergens, setSelectedAllergens] = useState([]);
  const ip = API_BASE_URL.replace(/[';]/g, '');

  const PackagingDict = {
    0: 'Los',
    1: 'Pak',
    2: 'Duo-pack',
    3: 'Doos',
    4: '4-pack',
    5: 'Fles',
    6: '6-pack',
    7: 'Blik',
    8: 'Kuipje',
    9: 'Zak',
    10: 'Net',
    11: 'Pot',
    12: 'Plastic zak',
    13: 'Plastic verpakking'
  };

  const ProductTypeDict = {
    1: 'Milliliter',
    2: 'Gram',
    3: 'Stuks',
  };

  const handleSave = async () => {
    console.log('Naam: ' + name);
    console.log('Description: ' + description);
    console.log('Hoeveelheid: ' + quantity);
    console.log('Minimum hoeveelheid: ' + minQuantity);
    console.log('Type: ' + selectedType);
    console.log('Geselecteerde allergenen: ' + selectedAllergens);
    console.log('Verpakking: ' + selectedPackaging);

    try {
      const data = {
        Name: name,
        Price: price,
        Calories: 100,
        Amount: quantity,
        SmallestAmount: minQuantity,
        Packaging: selectedPackaging,
        IngredientType: selectedType,
        ImageObjId: 1,
        Description: description,
      };

      const response = await fetch(`${ip}/api/Products`, {
        method: 'POST',
        header: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(data),
      });

      if (response.ok) {
        console.log('Product saved succesfully');
      } else {
        console.error('Failed to save product:', response.statusText);
      }
    } catch (error) {
      console.error('An error occurred while saving the product:', error.message);
    }

    console.log('Product bewaard!');
  };

  const handleInputNumber = (inputValue) => {
    const parsedValue = parseFloat(inputValue);

    if (!isNaN(parsedValue) || inputValue === '') {
      setQuantity(inputValue);
    }
  }

  const handleQuantityIncrement = () => {
    let tempValue = parseFloat(quantity);
    tempValue += 1;
    setQuantity(tempValue);
  };

  const handleQuantityDecrement = () => {
    let tempValue = parseFloat(quantity);
    tempValue -= 1;
    if (tempValue > 0) {
      setQuantity(quantity);
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
        <Text style={[styles.heading, styles.extramargintop]}>Naam</Text>
        <TextInput 
          style={[styles.textinput, styles.textinputname]}
          value={name}
          onChangeText={(text) => setName(text)}
        />

        <Text style={styles.heading}>Beschrijving</Text>
        <TextInput 
          style={[styles.textinput, styles.multilinetext]} 
          multiline
          value={description}
          onChangeText={(text) => setDescription(text)}
        />

        <Text style={styles.heading}>Kwantiteit</Text>
        <View style={styles.horizontalcontainer}>
          <View style={styles.quantitycontainer}>
            <TouchableOpacity onPress={handleQuantityDecrement}>
              <Text style={[styles.quantitybutton, styles.redquantitybutton]}>-</Text>
            </TouchableOpacity>
            <TextInput 
              style={[styles.input, styles.quantityinput]}
              value={quantity}
              onChangeText={handleInputNumber}
              keyboardType='decimal-pad'
            />
            <TouchableOpacity onPress={handleQuantityIncrement}>
              <Text style={[styles.quantitybutton, styles.greenquantitybutton]}>+</Text>
            </TouchableOpacity>
          </View>
          <Text style={styles.heading}>{ProductTypeDict[selectedType]}</Text>
        </View>

        {/* <Text style={styles.heading}>Minimum kwantiteit</Text>
        <View style={styles.horizontalcontainer}>
          <View style={styles.quantitycontainer}>
            <TouchableOpacity onPress={handleMinQuantityDecrement}>
              <Text style={[styles.quantitybutton, styles.redquantitybutton]}>-</Text>
            </TouchableOpacity>
            <TextInput 
              style={[styles.input, styles.quantityinput]}
              value={minQuantity}
              onChangeText={(text) => {
                const tempValue = text.replace(/[^0-9.]/g, '');
                if (tempValue === '') {
                  setMinQuantity(0);
                } else {
                  setMinQuantity(tempValue);
                }
              }}
              keyboardType='decimal-pad'
            />
            <TouchableOpacity onPress={handleMinQuantityIncrement}>
              <Text style={[styles.quantitybutton, styles.greenquantitybutton]}>+</Text>
            </TouchableOpacity>
          </View>
          <Text style={styles.heading}>{ProductTypeDict[selectedType]}</Text>
        </View> */}

        <Text style={styles.heading}>Type</Text>
        <Picker
          style={styles.picker}
          selectedValue={selectedType}
          onValueChange={(itemValue, itemIndex) => setSelectedType(itemValue)}>
          {Object.keys(ProductTypeDict).map((key) => (
            <Picker.Item key={key} label={ProductTypeDict[key]} value={parseInt(key)} />
          ))}
        </Picker>

        <Text style={styles.heading}>Verpakking</Text>
        <Picker
          style={styles.picker}
          selectedValue={selectedPackaging}
          onValueChange={(itemValue, itemIndex) => setSelectedPackaging(itemValue)}>
          {Object.keys(PackagingDict).map((key) => (
            <Picker.Item key={key} label={PackagingDict[key]} value={parseInt(key)} />
          ))}
        </Picker>

        <Text style={styles.heading}>Prijs</Text>
        <View style={styles.pricecontainer}>
          <Text style={styles.eurosign}>€</Text>
          <TextInput
            style={styles.priceinput}
            keyboardType='numeric'
            onChangeText={(text) => setPrice(text)}
          />
        </View>

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
      marginTop: 5,
    },
    eurosign: {
      fontSize: 20,
      marginRight: 5,
    },
    heading: {
      alignSelf: 'center',
      fontSize: 16,
      fontWeight: 'bold',
      marginBottom: -5,
    },
    horizontalcontainer: {
      flexDirection: 'row',
      justifyContent: 'center',
      alignItems: 'center',
    },
    input: {
      height: 40,
      margin: 12,
      padding: 10,
      borderWidth: 1,
      backgroundColor: '#ffffff',
    },
    pricecontainer: {
      alignSelf: 'center',
      flexDirection: 'row',
      alignItems: 'center',
      width: '30%',
      paddingHorizontal: 10,
    },
    priceinput: {
      flex: 1,
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
    picker: {
      alignSelf: 'center',
      margin: 12,
      width: '50%',
      backgroundColor: '#ffffff',
      borderWidth: 1,
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
    textinput: {
      height: 40,
      width: '85%',
      margin: 12,
      padding: 10,
      alignSelf: 'center',
      borderWidth: 1,
      backgroundColor: '#ffffff',
      fontSize: 16,
    },
    textinputname: {
      textAlign: 'center',
    }
});