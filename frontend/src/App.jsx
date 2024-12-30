import { Route, BrowserRouter as Router, Routes } from "react-router-dom"
import Footer from "./components/Footer"
import NavBar from "./components/NavBar"
import HomePage from "./pages/HomePage"
import ProductPage from "./pages/ProductPage"
import LoginPage from "./pages/LoginPage"
import { AuthProvider } from "./AuthContext"

function App() {
  return (
    <AuthProvider>
      <Router>
        <NavBar />
        <div className="container mx-auto p-4">
          <Routes>
            <Route path="/" element={<HomePage />} />
            <Route path="/product" element={<ProductPage />} />
            <Route path="/login" element={<LoginPage />} />
          </Routes>
        </div>

        <Footer />
      </Router>
    </AuthProvider>
  )
}

export default App
