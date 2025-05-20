// shopping_list_screen.dart
// Kullanıcının alışveriş listesi ekranı. Şu an örnek metin içeriyor.
// Tema renkleri ve dekoratif kutu ile modern bir görünüm sağlar.

import 'package:flutter/material.dart';

/// Alışveriş listesi ekranı: Kullanıcı burada alışveriş ürünlerini görecek.
class ShoppingListScreen extends StatelessWidget {
  const ShoppingListScreen({super.key});

  @override
  Widget build(BuildContext context) {
    // Tema renkleri
    const Color tropicalLime = Color(0xFFA3EBB1);
    const Color deepFern = Color(0xFF52796F);
    const Color midnightBlue = Color(0xFF2C3E50);
    const Color whiteSmoke = Color(0xFFF5F5F5);
    const Color oliveShadow = Color(0xFF6B705C);

    return Scaffold(
      backgroundColor: midnightBlue,
      appBar: AppBar(
        backgroundColor: deepFern.withOpacity(0.95),
        elevation: 0,
        centerTitle: true,
        title: ShaderMask(
          shaderCallback: (bounds) => LinearGradient(
            colors: [tropicalLime, Colors.white],
            begin: Alignment.topLeft,
            end: Alignment.bottomRight,
          ).createShader(bounds),
          child: const Text(
            'Alışveriş Listem',
            style: TextStyle(
              color: Colors.white,
              fontWeight: FontWeight.bold,
              fontSize: 28,
              letterSpacing: 1.2,
            ),
          ),
        ),
      ),
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            colors: [
              tropicalLime,
              deepFern,
              midnightBlue,
            ],
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
          ),
        ),
        child: SafeArea(
          child: Padding(
            padding: const EdgeInsets.all(20.0),
            child: Container(
              decoration: BoxDecoration(
                color: whiteSmoke.withOpacity(0.92),
                borderRadius: BorderRadius.circular(36),
                boxShadow: [
                  BoxShadow(
                    color: oliveShadow.withOpacity(0.15),
                    blurRadius: 20,
                    offset: const Offset(0, 5),
                  ),
                ],
              ),
              padding: const EdgeInsets.all(24),
              child: const Center(
                child: Text(
                  'Bu sayfada alışveriş listesi olacak',
                  style: TextStyle(
                    fontSize: 18,
                    color: Color(0xFF263238),
                  ),
                ),
              ),
            ),
          ),
        ),
      ),
    );
  }
}
