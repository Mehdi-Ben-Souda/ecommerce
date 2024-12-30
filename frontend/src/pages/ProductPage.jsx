import React, { useContext, useState } from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
//import { toast } from 'react-hot-toast';
import { ShoppingCart, Minus, Plus, Heart } from 'lucide-react';
import { AuthContext } from '../AuthContext';

const ProductPage = () => {
    const location = useLocation();
    const { product } = location.state;
    const [quantity, setQuantity] = useState(1);
    const [isWishlisted, setIsWishlisted] = useState(false);
    const [isAddingToCart, setIsAddingToCart] = useState(false);
    const { isAuthenticated } = useContext(AuthContext);
    const navigate = useNavigate();

    const handleQuantityChange = (action) => {
        if (action === 'increment') {
            setQuantity(prev => prev + 1);
        } else if (action === 'decrement' && quantity > 1) {
            setQuantity(prev => prev - 1);
        }
    };

    const handleAddToCart = async () => {
        if (!isAuthenticated) {
            //history.push('/login');
            navigate('/login');
            return;
        }
        try {
            setIsAddingToCart(true);
            // Here we'd make an API call to add to cart based on our CartItem model
            const cartItem = {
                ProductId: product.id,
                quantity: quantity
            };
            
            // Simulating API call
            await new Promise(resolve => setTimeout(resolve, 1000));
            
            //toast.success(`Added ${quantity} ${product.name} to cart!`);
            setIsAddingToCart(false);
        } catch (error) {
            //toast.error('Failed to add to cart. Please try again.');
            setIsAddingToCart(false);
        }
    };

    return (
        <div className="container mx-auto p-6">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-8">
                {/* Product Image Section */}
                <div className="relative">
                    <img
                        src={product.image || '/api/placeholder/600/400'}
                        alt={product.name}
                        className="w-full h-96 object-cover rounded-xl shadow-lg"
                    />
                    <button
                        onClick={() => setIsWishlisted(!isWishlisted)}
                        className="absolute top-4 right-4 p-2 bg-white rounded-full shadow-md hover:bg-gray-50 transition-colors"
                    >
                        <Heart 
                            className={isWishlisted ? 'text-red-500 fill-red-500' : 'text-gray-400'} 
                            size={24}
                        />
                    </button>
                </div>

                {/* Product Details Section */}
                <div className="space-y-6">
                    <div>
                        <h1 className="text-4xl font-bold text-gray-800 mb-2">{product.name}</h1>
                        <p className="text-3xl font-semibold text-green-600">${product.price.toFixed(2)}</p>
                    </div>

                    <div className="prose max-w-none">
                        <p className="text-gray-600">{product.description}</p>
                    </div>

                    {/* Quantity Selector */}
                    <div className="flex items-center space-x-4">
                        <span className="text-gray-700 font-medium">Quantity:</span>
                        <div className="flex items-center border rounded-lg overflow-hidden">
                            <button
                                onClick={() => handleQuantityChange('decrement')}
                                className="p-2 hover:bg-gray-100 transition-colors"
                                disabled={quantity <= 1}
                            >
                                <Minus size={20} className={quantity <= 1 ? 'text-gray-300' : 'text-gray-600'} />
                            </button>
                            <span className="px-4 py-2 font-medium">{quantity}</span>
                            <button
                                onClick={() => handleQuantityChange('increment')}
                                className="p-2 hover:bg-gray-100 transition-colors"
                            >
                                <Plus size={20} className="text-gray-600" />
                            </button>
                        </div>
                    </div>

                    {/* Add to Cart Button */}
                    <button
                        onClick={handleAddToCart}
                        disabled={isAddingToCart}
                        className="w-full md:w-auto px-8 py-3 bg-blue-600 hover:bg-blue-700 text-white rounded-lg font-medium 
                                 shadow-md transition-colors flex items-center justify-center space-x-2 disabled:bg-blue-400"
                    >
                        <ShoppingCart size={20} />
                        <span>{isAddingToCart ? 'Adding...' : 'Add to Cart'}</span>
                    </button>

                    {/* Additional Product Info */}
                    <div className="border-t pt-6 mt-6">
                        <h3 className="text-lg font-semibold mb-4">Product Details</h3>
                        <ul className="space-y-2 text-gray-600">
                            <li>Product ID: {product.id}</li>
                            <li>In Stock</li>
                            <li>Free Shipping Available</li>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default ProductPage;