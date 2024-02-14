import { StatusBar } from 'expo-status-bar';
import { StyleSheet, Text, View, SafeAreaView } from 'react-native';
import { TextInput } from 'react-native-gesture-handler';

const AddProductScreen = () => {
    //const [title, setTitle] = useState("");

    return (
      <SafeAreaView style={styles.container}>
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
      </SafeAreaView>
    );
}

export default AddProductScreen;

const styles = StyleSheet.create({
    container: {
      flex: 1,
      backgroundColor: '#fff0d6',
      alignItems: 'center',
      paddingTop: StatusBar.currentHeight,
    },
    extraMarginTop: {
      marginTop: 15,
    },
    heading: {
      fontSize: 16,
      fontWeight: 'bold',
    },
    input: {
      height: 40,
      width: '85%',
      margin: 12,
      padding: 10,
      borderWidth: 1,
      backgroundColor: '#ffffff',
    },
    multilineText: {
      minHeight: 80,
      textAlignVertical: "top",
    },
  });