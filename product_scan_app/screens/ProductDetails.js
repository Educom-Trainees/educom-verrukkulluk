import React from 'react';
import { View, Text, Image, StyleSheet } from 'react-native';
import { SafeAreaView } from 'react-native-safe-area-context';
import { GetPackagingValue } from '../constants/PackagingDict';
import { GetProductType } from '../constants/ProductTypeDict';
import { TouchableOpacity } from 'react-native-gesture-handler';

const images = {
    witte_bol: require('../assets/images/witte_bol.jpg'),
    hamburgers: require('../assets/images/hamburgers.png'),
    augurken: require('../assets/images/augurken.jpg'),
}

const ProductDetailsScreen = ({ route }) => {
    const { product } = route.params;

    const renderAllergies = () => {
        if (Array.isArray(product.productAllergies) && product.productAllergies.length > 0) {
            return (
                <View>
                    {product.productAllergies.map((productAllergy, index) => (
                        <View key={index}>
                            <Text>{productAllergy.allergy.name}</Text>
                        </View>
                    ))}
                </View>                
            );
        } else {
            return <Text>Geen allergieën</Text>;
        }
    }

    return (
        <SafeAreaView style={styles.container}>
            {/*<Image source={images[product.picture]} style={styles.image} resizeMode="contain" />*/}

            <Text style={styles.heading}>Naam</Text>
            <Text style={styles.text}>{product.name}</Text>

            <Text style={styles.heading}>Beschrijving</Text>
            <Text style={styles.text}>{product.description}</Text>

            <Text style={styles.heading}>Type</Text>
            <Text style={styles.text}>{GetProductType(product.ingredientType)}</Text>

            <Text style={styles.heading}>Kwantiteit</Text>
            <Text style={styles.text}>{product.amount}</Text>

            <Text style={styles.heading}>Minimum kwantiteit</Text>
            {/*<Text style={styles.text}>{product.minimumQuantity}</Text>*/}

            <Text style={styles.heading}>Verpakking</Text>
            {/*<Text style={styles.text}>{GetPackagingValue(product.packaging)}</Text>*/}

            <Text style={styles.heading}>Prijs</Text>
            <Text style={styles.text}>€{product.price}</Text>

            <Text style={styles.heading}>Allergieën</Text>
            {renderAllergies()}

            {/*<TouchableOpacity style={styles.editButton} onPress={handleEdit}>
                <Text style={styles.editButtonText}>Edit</Text>
            </TouchableOpacity>*/}
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
    editButton: {
        alignSelf: 'center',
        backgroundColor: '#95b82d',
        padding: 10,
        borderRadius: 5,
        marginTop: 20,
        width: '80%',
    },
    editButtonText: {
        color: 'white',
        textAlign: 'center',
        fontSize: 16,
        fontWeight: 'bold',
    },
    text: {
        fontSize: 16,
        marginBottom: 10,
    }
});