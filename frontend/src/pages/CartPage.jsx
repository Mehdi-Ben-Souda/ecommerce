// CartPage.jsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import CartItem from "../components/CartItem";
import EmptyCart from "../components/EmptyCart";
import OrderSummary from "../components/OrderSummary";

// Test data that matches your C# models structure
const initialCartItems = [
  {
    id: 1,
    product: {
      id: 1,
      name: "Gaming Laptop",
      price: 1299.99,
      description: "High-performance gaming laptop",
      image: "https://picsum.photos/200/300",
    },
    quantity: 1,
  },
  {
    id: 2,
    product: {
      id: 2,
      name: "Wireless Mouse",
      price: 49.99,
      description: "Ergonomic wireless mouse",
      image: "https://picsum.photos/200/300",
    },
    quantity: 2,
  },
  {
    id: 3,
    product: {
      id: 3,
      name: "Mechanical Keyboard",
      price: 129.99,
      description: "RGB mechanical keyboard",
      image: "https://picsum.photos/200/300",
    },
    quantity: 1,
  },
];

const CartPage = () => {
  const [cartItems, setCartItems] = useState(initialCartItems);
  const navigate = useNavigate();

  const handleRemoveItem = (itemId) => {
    setCartItems(cartItems.filter((item) => item.id !== itemId));
  };

  const handleUpdateQuantity = (itemId, action) => {
    setCartItems(
      cartItems.map((item) => {
        if (item.id === itemId) {
          const newQuantity =
            action === "increment"
              ? item.quantity + 1
              : Math.max(1, item.quantity - 1);
          return { ...item, quantity: newQuantity };
        }
        return item;
      })
    );
  };

  const handleCheckout = () => {
    console.log("Proceeding to checkout with items:", cartItems);
    // Navigate to checkout page
    // navigate('/checkout');
  };

  const handleContinueShopping = () => {
    navigate("/");
  };

  if (cartItems.length === 0) {
    return <EmptyCart onContinueShopping={handleContinueShopping} />;
  }

  return (
    <div className="min-h-screen bg-base-200 py-8 w-full">
      <div className="container mx-auto px-4 w-full items-center">
        <div className="text-center mb-8">
          <h1 className="text-4xl font-bold">Shopping Cart</h1>
          <p className="text-base-content/70 mt-2">
            {cartItems.length} {cartItems.length === 1 ? "item" : "items"} in
            your cart
          </p>
        </div>

        <div className="flex justify-center gap-8 w-full">
          {/* Cart Items List */}
          <div className="flex-grow">
            <div className="space-y-4">
              {cartItems.map((item) => (
                <CartItem
                  key={item.id}
                  item={item}
                  onRemove={handleRemoveItem}
                  onUpdateQuantity={handleUpdateQuantity}
                />
              ))}
            </div>
          </div>

          {/* Order Summary */}
          <div className="lg:w-96">
            <OrderSummary cartItems={cartItems} onCheckout={handleCheckout} />
          </div>
        </div>
      </div>
    </div>
  );
};

export default CartPage;
