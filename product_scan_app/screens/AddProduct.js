import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, TouchableOpacity } from 'react-native';
import { ScrollView, TextInput } from 'react-native-gesture-handler';
import { useState } from 'react';

const AddProductScreen = () => {
    const [quantity, setQuantity] = useState(0);
    const [minQuantity, setMinQuantity] = useState(0);

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


    return (
      <SafeAreaView style={styles.container}>
        <ScrollView contentContainerStyle={styles.scrollView} showVerticalScrollIndicator={false}>
          <Text style={[styles.heading, styles.extraMarginTop]}>Titel</Text>
          <TextInput style={styles.input} />

          <Text style={styles.heading}>Beschrijving</Text>
          <TextInput style={[styles.input, styles.multilineText]} multiline/>

          <Text style={styles.heading}>Type</Text>
          <TextInput style={styles.input} />

          <Text style={styles.heading}>Kwantiteit</Text>
          <View style={styles.quantityContainer}>
            <TouchableOpacity onPress={handleQuantityDecrement}>
              <Text style={[styles.quantityButton, styles.redquantitybutton]}>-</Text>
            </TouchableOpacity>
            <TextInput 
              style={[styles.input, styles.quantityinput]}
              value={quantity.toString()}
              onChangeText={text => setQuantity(parseFloat(text))}
              keyboardType='decimal-pad'
            />
            <TouchableOpacity onPress={handleQuantityIncrement}>
              <Text style={[styles.quantityButton, styles.greenquantitybutton]}>+</Text>
            </TouchableOpacity>
          </View>

          <Text style={styles.heading}>Minimum kwantiteit</Text>
          <View style={styles.quantityContainer}>
            <TouchableOpacity onPress={handleMinQuantityDecrement}>
              <Text style={[styles.quantityButton, styles.redquantitybutton]}>-</Text>
            </TouchableOpacity>
            <TextInput 
              style={[styles.input, styles.quantityinput]}
              value={minQuantity.toString()}
              onChangeText={text => setMinQuantity(parseFloat(text))}
              keyboardType='decimal-pad'
            />
            <TouchableOpacity onPress={handleMinQuantityIncrement}>
              <Text style={[styles.quantityButton, styles.greenquantitybutton]}>+</Text>
            </TouchableOpacity>
          </View>

          <Text style={styles.heading}>Verpakking</Text>
          <TextInput style={styles.input} />

          <Text style={styles.heading}>Prijs</Text>
          <TextInput
            style={styles.input}
            keyboardType='numeric'
          />

          <Text style={styles.heading}>Allergieën</Text>
          <TextInput style={styles.input} />

          <TouchableOpacity style={styles.saveButton} onPress={handleSave}>
            <Text style={styles.saveButtonText}>Opslaan</Text>
          </TouchableOpacity>
        </ScrollView>
      </SafeAreaView>
    );
}

export default AddProductScreen;

const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: '#fff0d6',
      paddingTop: StatusBar.currentHeight,
    },
    scrollView: {
      backgroundColor: '#fff0d6',
      paddingTop: 15,
      paddingBottom: 60,
    },
    extraMarginTop: {
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
    multilineText: {
      minHeight: 80,
      textAlignVertical: "top",
    },
    quantityContainer: {
      flexDirection: 'row',
      alignItems: 'center',
      justifyContent: 'center',
      paddingHorizontal: 10,
    },
    quantityButton: {
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
    saveButton: {
      alignSelf: 'center',
      backgroundColor: '#95b82d',
      padding: 10,
      borderRadius: 5,
      marginTop: 20,
      width: '80%',
    },
    saveButtonText: {
      color: 'white',
      textAlign: 'center',
      fontSize: 16,
      fontWeight: 'bold',
    },
  });