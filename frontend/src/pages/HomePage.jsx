import { useEffect, useState } from "react";
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import ProductCard from "../components/ProductCard";

const HomePage = () => {
    const [products, setProducts] = useState([]);
    const navigate = useNavigate();

    useEffect(() => {
        axios.get(`http://localhost:5000/catalog`)
            .then(response => setProducts(response.data))
            .catch(error => console.log(error));
    }, []); // eslint-disable-line react-hooks/exhaustive-deps

    const handleProductClick = (product) => {
        navigate(`/product`, { state: { product } });
    };

    return (
        <div className="container mx-auto p-4">
            
            <h1 className="text-4xl font-bold text-center mb-8">Welcome to the Home e-commerce</h1>
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-8">
                {products.map(product => (
                    <ProductCard key={product._id} product={product} onClick={() => handleProductClick(product)} />
                ))}
            </div>
        </div>
    );
};

export default HomePage;