# QuyLopWinform

Ứng dụng **quản lý quỹ lớp** xây dựng bằng **C# WinForms** với mô hình tách lớp `BLL` / `DAL` / `UI`, hỗ trợ quản lý tài khoản, lớp học, thành viên, khoản thu, khoản chi và tổng quan số dư.

---

## 1. Mục tiêu đề tài

Phần mềm được xây dựng nhằm hỗ trợ:

- quản lý quỹ lớp theo từng lớp học
- quản lý thành viên và phân quyền trong lớp
- theo dõi các khoản thu, khoản chi
- tổng hợp số dư quỹ lớp theo thời gian thực
- hỗ trợ thao tác trực quan qua giao diện WinForms

---

## 2. Công nghệ sử dụng

- **Ngôn ngữ:** C#
- **Nền tảng giao diện:** Windows Forms (.NET Framework)
- **Cơ sở dữ liệu:** SQL Server
- **Truy cập dữ liệu:** LINQ to SQL / DataClasses
- **IDE phát triển:** Visual Studio
- **Quản lý mã nguồn:** GitHub

---

## 3. Cấu trúc project

```text
QuyLopWinform/
├─ LopFund.BLL/        # Business Logic Layer
├─ LopFund.DAL/        # Data Access Layer
├─ LopFund.UI/         # UI dùng chung / thành phần hỗ trợ
├─ QuyLopWinform/      # Ứng dụng WinForms chính
└─ QuyLopWinform.slnx  # Solution
```

### Giải thích ngắn

- **LopFund.BLL**: xử lý nghiệp vụ như đăng nhập, đăng ký, quản lý lớp, quản lý thành viên, khoản thu, khoản chi.
- **LopFund.DAL**: làm việc với cơ sở dữ liệu SQL Server.
- **QuyLopWinform**: chứa các form giao diện và luồng tương tác chính của hệ thống.

---

## 4. Chức năng chính

### 4.1. Quản lý tài khoản
- Đăng ký tài khoản
- Đăng nhập
- Đăng xuất
- Lưu phiên làm việc bằng `AppSession`

### 4.2. Quản lý lớp học
- Tạo lớp mới
- Tham gia lớp bằng mã mời
- Chọn lớp để làm việc
- Đổi lớp đang thao tác
- Xem danh sách lớp đã tham gia
- Quản lý lớp
- Sửa thông tin lớp
- Xóa lớp

### 4.3. Quản lý thành viên
- Xem danh sách thành viên trong lớp
- Thêm thành viên
- Sửa thông tin thành viên
- Xóa/ẩn thành viên
- Nâng quyền lên Admin
- Hạ quyền về Member

### 4.4. Quản lý khoản thu
- Tạo khoản thu mới
- Thiết lập số tiền cần nộp
- Tùy chọn có hoặc không có hạn nộp
- Quản lý danh sách thành viên đã nộp
- Check all / bỏ chọn tất cả
- Tính tổng phải thu, đã thu, còn lại

### 4.5. Quản lý khoản chi
- Thêm khoản chi
- Sửa khoản chi
- Xóa khoản chi
- Xem danh sách khoản chi

### 4.6. Tổng quan quỹ lớp
- Hiển thị tổng thu
- Hiển thị tổng chi
- Hiển thị số dư hiện tại
- Đồng bộ số liệu theo lớp đang chọn

---

## 5. Các form chính trong hệ thống

- `FrmLogin` — Đăng nhập
- `FrmRegister` — Đăng ký tài khoản
- `FrmClassPicker` — Chọn lớp / tạo lớp / tham gia lớp
- `FrmClassrooms` — Quản lý lớp và phân quyền
- `FrmMain` — Trang tổng quan quỹ lớp
- `FrmMemberEdit` — Thêm / sửa thành viên
- `FrmFeeCycleAdd` — Tạo khoản thu
- `FrmInvoicePayments` — Quản lý danh sách nộp tiền
- `FrmExpenseAdd` — Thêm / sửa khoản chi
- `FrmExpenses` — Quản lý khoản chi

---

## 6. Luồng hoạt động của hệ thống

### Luồng 1: Người dùng mới
1. Mở ứng dụng
2. Vào màn hình **Đăng nhập**
3. Chọn **Đăng ký**
4. Nhập họ tên, email, số điện thoại, mật khẩu
5. Đăng ký thành công
6. Quay lại đăng nhập
7. Đăng nhập bằng tài khoản vừa tạo

### Luồng 2: Sau đăng nhập
1. Hệ thống lưu thông tin user hiện tại
2. Mở form **Chọn lớp**
3. Người dùng có thể:
   - tạo lớp mới
   - tham gia lớp bằng mã mời
   - chọn lớp đã có
4. Hệ thống lưu lớp đang thao tác
5. Mở **Trang chính**

### Luồng 3: Quản lý quỹ lớp
1. Xem tổng quan số dư
2. Xem danh sách thành viên
3. Thêm hoặc sửa thành viên
4. Tạo khoản thu
5. Mở danh sách nộp tiền
6. Đánh dấu thành viên đã nộp
7. Thêm khoản chi
8. Quay lại trang chính để xem số dư cập nhật

### Luồng 4: Quản lý lớp
1. Mở **Quản lý lớp**
2. Chọn lớp cần thao tác
3. Xem danh sách user trong lớp
4. Nâng quyền Admin hoặc hạ quyền Member

### Luồng 5: Đăng xuất
1. Chọn **Đăng xuất**
2. Xác nhận đăng xuất
3. Xóa session hiện tại
4. Quay lại màn hình đăng nhập

---

## 7. Phân quyền người dùng

Hệ thống áp dụng 3 vai trò chính:

- **Owner**
  - tạo và quản lý lớp
  - sửa/xóa lớp
  - nâng/hạ quyền người dùng
  - quản lý khoản thu, khoản chi, thành viên

- **Admin**
  - được phép thao tác quản lý trong lớp theo quyền hệ thống cho phép

- **Member**
  - chủ yếu dùng để xem thông tin lớp và dữ liệu quỹ
  - không có quyền quản trị nâng cao

---

## 8. Hướng dẫn cài đặt và chạy project

### 8.1. Clone source code

```bash
git clone https://github.com/dat01102004/QuyLopWinform.git
```

### 8.2. Mở project
- Mở file solution bằng Visual Studio
- Restore NuGet packages nếu được yêu cầu

### 8.3. Cài đặt cơ sở dữ liệu
- Mở **SQL Server Management Studio**
- Tạo database mới, ví dụ: `LopFundWinform`
- Chạy file script SQL của project nếu đã được cung cấp

Ví dụ:

```sql
CREATE DATABASE LopFundWinform;
GO
USE LopFundWinform;
GO
```

Sau đó chạy script tạo bảng và dữ liệu mẫu.

### 8.4. Kiểm tra chuỗi kết nối
Mở file cấu hình của ứng dụng và chỉnh lại `Data Source`, `Initial Catalog` cho phù hợp với máy đang dùng.

Ví dụ:

```xml
Data Source=.\SQLEXPRESS;Initial Catalog=LopFundWinform;Integrated Security=True
```

### 8.5. Chạy ứng dụng
- Chọn project `QuyLopWinform` làm Startup Project
- Nhấn **Start** hoặc `F5`

---

## 9. Hướng dẫn xuất và chia sẻ database

Để chia sẻ database cho thành viên khác trong nhóm:

1. Mở SSMS
2. Chuột phải vào database
3. Chọn **Tasks → Generate Scripts**
4. Chọn **Schema and data**
5. Lưu thành file `.sql`
6. Đưa file đó vào thư mục `Database/` trong repo

Gợi ý cấu trúc:

```text
QuyLopWinform/
├─ Database/
│  └─ LopFundWinform.sql
└─ QuyLopWinform/
```

---

## 10. Ảnh giao diện nên đưa vào báo cáo

### Nhóm 1: Tài khoản
- Màn hình đăng nhập
- Màn hình đăng ký
- Thông báo đăng ký thành công
- Thông báo đăng nhập lỗi / đúng

### Nhóm 2: Lớp học
- Chọn lớp
- Tạo lớp
- Tham gia lớp bằng mã mời
- Quản lý lớp
- Phân quyền thành viên

### Nhóm 3: Trang chính
- Tổng quan quỹ lớp
- Danh sách thành viên
- Khối khoản thu
- Khối khoản chi

### Nhóm 4: Khoản thu
- Tạo khoản thu
- Danh sách nộp tiền
- Check all / bỏ chọn hết
- Tổng phải thu / đã thu / còn lại

### Nhóm 5: Khoản chi
- Thêm khoản chi
- Sửa khoản chi
- Quản lý khoản chi

---

## 11. Kết quả đạt được

- Hoàn thành module đăng nhập / đăng ký
- Hoàn thành module chọn lớp, tạo lớp, tham gia lớp
- Hoàn thành module phân quyền người dùng theo vai trò
- Hoàn thành module quản lý thành viên
- Hoàn thành module quản lý khoản thu
- Hoàn thành module quản lý khoản chi
- Hoàn thành module tổng hợp số dư quỹ lớp
- Hoàn thiện giao diện WinForms theo hướng đồng bộ và dễ sử dụng hơn

---

## 12. Hạn chế và hướng phát triển

### Hạn chế
- Chưa có xuất Excel / PDF
- Chưa có tìm kiếm và lọc nâng cao
- Chưa có biểu đồ thống kê
- Chưa có xác thực email hoặc OTP
- Chưa có logging / audit chi tiết

### Hướng phát triển
- Bổ sung biểu đồ thu chi theo tháng
- Xuất báo cáo ra Excel / PDF
- Thêm chức năng tìm kiếm thành viên, khoản thu, khoản chi
- Hoàn thiện quản trị người dùng nâng cao
- Nâng cấp giao diện hiện đại hơn nữa

---

## 13. Thành viên thực hiện

- Họ tên sinh viên: ...
- Mã số sinh viên: ...
- Lớp: ...
- Giảng viên hướng dẫn: ...

---

## 14. Ghi chú

README này có thể tiếp tục cập nhật khi nhóm hoàn thiện thêm:
- script cơ sở dữ liệu
- ảnh minh họa giao diện
- video demo
- tài liệu báo cáo học phần

