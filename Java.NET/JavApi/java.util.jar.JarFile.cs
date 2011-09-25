/*
 *  Licensed under the Apache License, Version 2.0 (the "License");
 *  you may not use this file except in compliance with the License.
 *  You may obtain a copy of the License at 
 * 
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 *  Unless required by applicable law or agreed to in writing, software
 *  distributed under the License is distributed on an "AS IS" BASIS,
 *  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *  See the License for the specific language governing permissions and
 *  limitations under the License.
 */
using System;
using java = biz.ritter.javapi;

namespace biz.ritter.javapi.util.jar
{
    /**
     * {@code JarFile} is used to read jar entries and their associated data from
     * jar files.
     *
     * @see JarInputStream
     * @see JarEntry
     */
    public class JarFile : java.util.zip.ZipFile
    {

        // The directory containing the manifest.
        const String META_DIR = "META-INF/"; //$NON-NLS-1$
        /**
         * The MANIFEST file name.
         */
        public const String MANIFEST_NAME = META_DIR+"MANIFEST.MF"; //$NON-NLS-1$
        // The manifest after it has been read from the JAR.
        private Manifest manifest;

        // The entry for the MANIFEST.MF file before it is read.
        private java.util.zip.ZipEntry manifestEntry;

        internal JarVerifier verifier;

        private bool closed = false;

        
    /**
     * Create a new {@code JarFile} using the contents of the specified file.
     *
     * @param file
     *            the JAR file as {@link File}.
     * @throws IOException
     *             If the file cannot be read.
     */
    public JarFile(java.io.File file) ://throws IOException {
        this(file, true){
    }

    /**
     * Create a new {@code JarFile} using the contents of the specified file.
     *
     * @param file
     *            the JAR file as {@link File}.
     * @param verify
     *            if this JAR file is signed whether it must be verified.
     * @throws IOException
     *             If the file cannot be read.
     */
    public JarFile(java.io.File file, bool verify) //throws IOException {
        :base(file){
        if (verify) {
            verifier = new JarVerifier(file.getPath());
        }
        readMetaEntries();
    }

    /**
     * Create a new {@code JarFile} using the contents of file.
     *
     * @param file
     *            the JAR file as {@link File}.
     * @param verify
     *            if this JAR filed is signed whether it must be verified.
     * @param mode
     *            the mode to use, either {@link ZipFile#OPEN_READ OPEN_READ} or
     *            {@link ZipFile#OPEN_DELETE OPEN_DELETE}.
     * @throws IOException
     *             If the file cannot be read.
     */
    public JarFile(java.io.File file, bool verify, int mode) ://throws IOException {
        base(file, mode){
        if (verify) {
            verifier = new JarVerifier(file.getPath());
        }
        readMetaEntries();
    }

    /**
     * Create a new {@code JarFile} from the contents of the file specified by
     * filename.
     *
     * @param filename
     *            the file name referring to the JAR file.
     * @throws IOException
     *             if file name cannot be opened for reading.
     */
    public JarFile(String filename):// throws IOException {
        this(filename, true){
    }

    /**
     * Create a new {@code JarFile} from the contents of the file specified by
     * {@code filename}.
     *
     * @param filename
     *            the file name referring to the JAR file.
     * @param verify
     *            if this JAR filed is signed whether it must be verified.
     * @throws IOException
     *             If file cannot be opened or read.
     */
    public JarFile(String filename, bool verify) //throws IOException {
        :base(filename){
        if (verify) {
            verifier = new JarVerifier(filename);
        }
        readMetaEntries();
    }
        /**
         * Returns the {@code Manifest} object associated with this {@code JarFile}
         * or {@code null} if no MANIFEST entry exists.
         *
         * @return the MANIFEST.
         * @throws IOException
         *             if an error occurs reading the MANIFEST file.
         * @throws IllegalStateException
         *             if the jar file is closed.
         * @see Manifest
         */
        public Manifest getManifest(){// throws IOException {
            if (closed) {
                // archive.35=JarFile has been closed
                throw new java.lang.IllegalStateException("JarFile has been closed"); //$NON-NLS-1$
            }
            if (manifest != null) {
                return manifest;
            }
            try {
                java.io.InputStream isJ = base.getInputStream(manifestEntry);
                if (verifier != null) {
                    verifier.addMetaEntry(manifestEntry.getName(),
                            org.apache.harmony.luni.util.InputStreamHelper.readFullyAndClose(isJ));
                    isJ = base.getInputStream(manifestEntry);
                }
                try {
                    manifest = new Manifest(isJ, verifier != null);
                } finally {
                    isJ.close();
                }
                manifestEntry = null;  // Can discard the entry now.
            } catch (java.lang.NullPointerException e) {
                manifestEntry = null;
            }
            return manifest;
        }

        /**
         * Called by the JarFile constructors, this method reads the contents of the
         * file's META-INF/ directory and picks out the MANIFEST.MF file and
         * verifier signature files if they exist. Any signature files found are
         * registered with the verifier.
         * 
         * @throws IOException
         *             if there is a problem reading the jar file entries.
         */
        private void readMetaEntries(){// throws IOException {
            // Get all meta directory entries
            java.util.zip.ZipEntry[] metaEntries = getMetaEntriesImpl();
            if (metaEntries == null) {
                verifier = null;
                return;
            }

            bool signed = false;

            foreach (java.util.zip.ZipEntry entry in metaEntries) {
                String entryName = entry.getName();
                // Is this the entry for META-INF/MANIFEST.MF ?
                if (manifestEntry == null
                        && org.apache.harmony.archive.util.Util.asciiEqualsIgnoreCase(MANIFEST_NAME, entryName)) {
                    manifestEntry = entry;
                    // If there is no verifier then we don't need to look any further.
                    if (verifier == null) {
                        break;
                    }
                } else {
                    // Is this an entry that the verifier needs?
                    if (verifier != null
                            && (org.apache.harmony.archive.util.Util.asciiEndsWithIgnoreCase(entryName, ".SF")
                                    || org.apache.harmony.archive.util.Util.asciiEndsWithIgnoreCase(entryName, ".DSA")
                                    || org.apache.harmony.archive.util.Util.asciiEndsWithIgnoreCase(entryName, ".RSA"))) {
                        signed = true;
                        java.io.InputStream isJ = base.getInputStream(entry);
                        byte[] buf = org.apache.harmony.luni.util.InputStreamHelper.readFullyAndClose(isJ);
                        verifier.addMetaEntry(entryName, buf);
                    }
                }
            }

            // If there were no signature files, then no verifier work to do.
            if (!signed) {
                verifier = null;
            }
        }
        /**
         * Returns all the ZipEntry's that relate to files in the
         * JAR's META-INF directory.
         *
         * @return the list of ZipEntry's or {@code null} if there are none.
         */
        private java.util.zip.ZipEntry[] getMetaEntriesImpl() {
            List<java.util.zip.ZipEntry> list = new ArrayList<java.util.zip.ZipEntry>(8);
            Enumeration<java.util.zip.ZipEntry> allEntries = entries();
            while (allEntries.hasMoreElements()) {
                java.util.zip.ZipEntry ze = allEntries.nextElement();
                if (ze.getName().startsWith(META_DIR)
                        && ze.getName().length() > META_DIR.length()) {
                    list.add(ze);
                }
            }
            if (list.size() == 0) {
                return null;
            }
            java.util.zip.ZipEntry[] result = new java.util.zip.ZipEntry[list.size()];
            list.toArray(result);
            return result;
        }
    }
}
