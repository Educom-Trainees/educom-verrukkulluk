import React from 'react';
import { View, Text, Image, StyleSheet } from 'react-native';

const ProductDetailsScreen = ({ route }) => {
    const { product } = route.params;

    return (
        <View style={styles.container}>
            <Image source={product.image} style={styles.image} />
            <Text style={styles.title}>{product.title}</Text>
            <Text style={styles.title}>{product.description}</Text>
        </View>
    );
};

export default ProductDetailsScreen;

const styles = StyleSheet.create({
    container: {
        flex: 1,
        alignItems: 'center',
        justifyContent: 'center',
    },
    image: {
        width: 200,
        height: 200,
        marginBottom: 16,
    },
    title: {
        fontSize: 24,
        fontWeight: 'bold',
    }
});