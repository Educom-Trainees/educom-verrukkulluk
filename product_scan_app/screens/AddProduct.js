import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView, TouchableOpacity } from 'react-native';
import { ScrollView, TextInput } from 'react-native-gesture-handler';

const AddProductScreen = () => {
    //const [title, setTitle] = useState("");

    const handleSave = () => {
      console.log('Product bewaard!');
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
          <TextInput style={styles.input} />

          <Text style={styles.heading}>Minimum kwantiteit</Text>
          <TextInput style={styles.input} />

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