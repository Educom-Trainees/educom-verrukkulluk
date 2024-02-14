import React from 'react';
import { View, Text, Image, StyleSheet } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';

const images = {
    witte_bol: require('../assets/images/witte_bol.jpg'),
    hamburgers: require('../assets/images/hamburgers.png'),
    augurken: require('../assets/images/augurken.jpg'),
}

const ProductDetailsScreen = ({ route }) => {
    const { product } = route.params;

    return (
        <SafeAreaView style={styles.container}>
            <Image source={images[product.picture]} style={styles.image} resizeMode="contain" />

            <Text style={styles.heading}>Titel</Text>
            <Text style={styles.text}>{product.title}</Text>

            <Text style={styles.heading}>Beschrijving</Text>
            <Text style={styles.text}>{product.description}</Text>

            <Text style={styles.heading}>Type</Text>
            <Text style={styles.text}>{product.type}</Text>

            <Text style={styles.heading}>Kwantiteit</Text>
            <Text style={styles.text}>{product.quantity}</Text>

            <Text style={styles.heading}>Minimum kwantiteit</Text>
            <Text style={styles.text}>{product.minimumQuantity}</Text>

            <Text style={styles.heading}>Verpakking</Text>
            <Text style={styles.text}>{product.packaging}</Text>

            <Text style={styles.heading}>Prijs</Text>
            <Text style={styles.text}>{product.price}</Text>

            <Text style={styles.heading}>Allergieën</Text>
            <Text style={styles.text}>{product.allergies}</Text>
        </SafeAreaView>
    );
};

export default ProductDetailsScreen;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        backgroundColor: '#fff0d6',
        justifyContent: 'center',
        alignItems: 'center',
    },
    heading: {
        fontSize: 16,
        fontWeight: 'bold',
    },
    image: {
        width: 200,
        height: 200,
        marginBottom: 16,
    },
    text: {
        fontSize: 16,
        marginBottom: 10,
    }
});