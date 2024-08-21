-- phpMyAdmin SQL Dump
-- version 5.2.0
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Aug 21, 2024 at 07:09 AM
-- Server version: 10.4.27-MariaDB
-- PHP Version: 8.1.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `task3`
--

-- --------------------------------------------------------

--
-- Table structure for table `tableorder`
--

CREATE TABLE `tableorder` (
  `TableOrderId` int(11) NOT NULL,
  `TableId` varchar(36) NOT NULL,
  `MenuName` varchar(75) NOT NULL,
  `QuantityOrder` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tableorder`
--

INSERT INTO `tableorder` (`TableOrderId`, `TableId`, `MenuName`, `QuantityOrder`) VALUES
(1, '1a2b3c4d-5678-1234-5678-9abcdef01234', 'Spaghetti Carbonara', 2),
(2, '1a2b3c4d-5678-1234-5678-9abcdef01234', 'Caesar Salad', 1),
(3, '2b3c4d5e-6789-2345-6789-0bcdef012345', 'Margarita Pizza', 3),
(4, '3c4d5e6f-7890-3456-7890-1cdef0123456', 'Cappuccino', 2),
(5, '4d5e6f7g-8901-4567-8901-2def01234567', 'Grilled Chicken', 4);

-- --------------------------------------------------------

--
-- Table structure for table `tablespecification`
--

CREATE TABLE `tablespecification` (
  `TableId` varchar(36) NOT NULL,
  `TableNumber` int(11) DEFAULT NULL,
  `ChairNumber` int(11) DEFAULT NULL,
  `TablePic` varchar(75) NOT NULL,
  `TableType` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `tablespecification`
--

INSERT INTO `tablespecification` (`TableId`, `TableNumber`, `ChairNumber`, `TablePic`, `TableType`) VALUES
('1a2b3c4d-5678-1234-5678-9abcdef01234', 1, 4, 'table1.jpg', 'Dining'),
('2b3c4d5e-6789-2345-6789-0bcdef012345', 2, 6, 'table2.jpg', 'Lounge'),
('3c4d5e6f-7890-3456-7890-1cdef0123456', 3, 2, 'table3.jpg', 'Coffee'),
('4d5e6f7g-8901-4567-8901-2def01234567', 4, 8, 'table4.jpg', 'Dining'),
('5e6f7g8h-9012-5678-9012-3ef012345678', 5, 4, 'table5.jpg', 'Outdoor');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tableorder`
--
ALTER TABLE `tableorder`
  ADD PRIMARY KEY (`TableOrderId`),
  ADD KEY `TableId` (`TableId`);

--
-- Indexes for table `tablespecification`
--
ALTER TABLE `tablespecification`
  ADD PRIMARY KEY (`TableId`);

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tableorder`
--
ALTER TABLE `tableorder`
  ADD CONSTRAINT `tableorder_ibfk_1` FOREIGN KEY (`TableId`) REFERENCES `tablespecification` (`TableId`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
