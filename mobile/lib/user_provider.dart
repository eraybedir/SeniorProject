// user_provider.dart
// Kullanıcıya ait verilerin (isim, email, boy, kilo, yaş, avatar vb.) yönetimini sağlar.
// Veriler local storage (SharedPreferences) ile saklanır ve uygulama genelinde Provider ile erişilir.

import 'package:flutter/material.dart';
import 'package:shared_preferences/shared_preferences.dart';

/// Kullanıcı verilerini ve ayarlarını yöneten Provider sınıfı.
class UserProvider with ChangeNotifier {
  // Kullanıcı bilgileri
  String _name = '';
  String _email = '';
  double _height = 0;
  double _weight = 0;
  int _age = 0;
  String _gender = '';
  String _activityLevel = '';
  String _goal = '';
  String _avatar = '🍏'; // Varsayılan avatar

  // Getter'lar
  String get name => _name;
  String get email => _email;
  double get height => _height;
  double get weight => _weight;
  int get age => _age;
  String get gender => _gender;
  String get activityLevel => _activityLevel;
  String get goal => _goal;
  String get avatar => _avatar;

  /// Avatarı günceller ve kaydeder.
  void setAvatar(String newAvatar) {
    _avatar = newAvatar;
    saveUserData(avatar: newAvatar);
    notifyListeners();
  }

  /// İsmi günceller ve kaydeder.
  void setName(String newName) {
    _name = newName;
    saveUserData(name: newName);
    notifyListeners();
  }

  /// Local storage'dan kullanıcı verilerini yükler.
  Future<void> loadUserData() async {
    final prefs = await SharedPreferences.getInstance();
    _name = prefs.getString('name') ?? '';
    _email = prefs.getString('email') ?? '';
    _height = prefs.getDouble('height') ?? 0;
    _weight = prefs.getDouble('weight') ?? 0;
    _age = prefs.getInt('age') ?? 0;
    _gender = prefs.getString('gender') ?? '';
    _activityLevel = prefs.getString('activityLevel') ?? '';
    _goal = prefs.getString('goal') ?? '';
    _avatar = prefs.getString('avatar') ?? '🍏';
    notifyListeners();
  }

  /// Kullanıcı verilerini local storage'a kaydeder.
  Future<void> saveUserData({
    String? name,
    String? email,
    double? height,
    double? weight,
    int? age,
    String? gender,
    String? activityLevel,
    String? goal,
    String? avatar,
  }) async {
    final prefs = await SharedPreferences.getInstance();

    if (name != null) {
      _name = name;
      await prefs.setString('name', name);
    }
    if (email != null) {
      _email = email;
      await prefs.setString('email', email);
    }
    if (height != null) {
      _height = height;
      await prefs.setDouble('height', height);
    }
    if (weight != null) {
      _weight = weight;
      await prefs.setDouble('weight', weight);
    }
    if (age != null) {
      _age = age;
      await prefs.setInt('age', age);
    }
    if (gender != null) {
      _gender = gender;
      await prefs.setString('gender', gender);
    }
    if (activityLevel != null) {
      _activityLevel = activityLevel;
      await prefs.setString('activityLevel', activityLevel);
    }
    if (goal != null) {
      _goal = goal;
      await prefs.setString('goal', goal);
    }
    if (avatar != null) {
      _avatar = avatar;
      await prefs.setString('avatar', avatar);
    }

    notifyListeners();
  }
}
